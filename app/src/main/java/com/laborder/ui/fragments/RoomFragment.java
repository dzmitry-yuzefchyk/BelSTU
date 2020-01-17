package com.laborder.ui.fragments;

import android.app.Dialog;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.database.DatabaseError;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;
import com.google.firebase.database.GenericTypeIndicator;
import com.google.firebase.database.ValueEventListener;
import com.laborder.MainActivity;
import com.laborder.R;
import com.laborder.bl.BackStack;
import com.laborder.bl.Documents;
import com.laborder.bl.adapters.StudentAdapter;
import com.laborder.bl.models.Order;
import com.laborder.bl.models.ReserveOrder;
import com.laborder.bl.models.Student;
import com.laborder.bl.models.UserInfo;
import com.laborder.databinding.RoomFragmentBinding;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class RoomFragment extends Fragment {
    private String id;
    private RoomFragmentBinding binding;
    private List<Student> students;
    private StudentAdapter adapter;
    private DatabaseReference database;
    private FirebaseAuth mAuth;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        Bundle bundle = this.getArguments();
        if (bundle != null) {
            id = bundle.getString("id", "");
        }
        mAuth = FirebaseAuth.getInstance();
        binding = DataBindingUtil.inflate(inflater, R.layout.room_fragment, container, false);
        binding.setReserve(new ReserveOrder());
        database = FirebaseDatabase.getInstance().getReference().child(Documents.Orders).child(id);
        students = new ArrayList<>();
        setupRecyclerView();
        configureDatabase();
        return binding.getRoot();
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
    }

    @Override
    public void onStart() {
        super.onStart();
        bindButtons();
    }

    private void bindButtons() {
        getView().findViewById(R.id.reserve_btn).setOnClickListener(v -> reserve());
        getView().findViewById(R.id.next_btn).setOnClickListener(v -> nextStudent());
        getView().findViewById(R.id.delete_btn).setOnClickListener(v -> deleteOrder());
    }

    private void deleteOrder() {
        Dialog mDialog;
        mDialog=new Dialog(getContext());
        mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
        mDialog.setContentView(R.layout.delete_dialog);
        Button ok= mDialog.findViewById(R.id.dialog_yes_btn);
        Button cancel = mDialog.findViewById(R.id.dialog_no_btn);
        ok.setOnClickListener(v -> {
            database.removeValue();
            FirebaseDatabase.getInstance().getReference()
                    .child(Documents.OrdersIds)
                    .child(id)
                    .removeValue();
            mDialog.cancel();
        });
        cancel.setOnClickListener(v -> mDialog.cancel());
        mDialog.show();
    }

    private void nextStudent() {
        Order order = binding.getOrder();
        if (order == null) return;

        if (order.getQueue() == null) return;

        if (order.getFinished() == null) {
            order.setFinished(new HashMap<>());
        }

        HashMap<String, Student> finished = order.getFinished();
        HashMap<String, Student> queue = order.getQueue();
        Optional<Map.Entry<String, Student>> currentStudent = queue
                .entrySet()
                .stream()
                .min((Map.Entry<String, Student> e1, Map.Entry<String, Student> e2) ->
                        e1.getValue().getPriority() - e2.getValue().getPriority()
                );

        if (currentStudent != null) {
            queue.remove(currentStudent.get().getKey());
        }

        finished.put(currentStudent.get().getKey(), currentStudent.get().getValue());
        order.setFinished(finished);
        order.setCurrent(currentStudent.get().getValue());
        database.setValue(order);
    }

    private void configureButtons() {
        if (getView() == null) {
            return;
        }

        if (binding.getOrder().getCreatorId().equals(mAuth.getUid())) {
            getView().findViewById(R.id.next_btn).setVisibility(View.VISIBLE);
            getView().findViewById(R.id.delete_btn).setVisibility(View.VISIBLE);
        } else {
            getView().findViewById(R.id.reserve_btn).setVisibility(View.VISIBLE);
            getView().findViewById(R.id.labs_et).setVisibility(View.VISIBLE);
        }
    }

    private void reserve() {
        String uid = mAuth.getUid();
        MainActivity activity = (MainActivity) getActivity();
        Order order = binding.getOrder();
        if (order == null) return;

        if (order.getQueue() == null) order.setQueue(new HashMap<>());

        HashMap<String, Student> queue = order.getQueue();
        if (queue.containsKey(uid)) {
            activity.toast(getResources().getString(R.string.to_reserve));
            return;
        }

        ArrayList<Integer> labs = parseLabs();
        if (labs == null) {
            return;
        }

        if (labs.size() == 0) {
            return;
        }

        if (binding.getOrder().isUsePriority()) {
            reserveWithPriority(parseLabs());
        } else {
            reserveWithoutPriority(parseLabs());
        }
    }

    private ArrayList<Integer> parseLabs() {
        MainActivity activity = (MainActivity) getActivity();
        ArrayList<Integer> labs;
        try {
            String labsString = binding.getReserve().getLabs();
            labsString = labsString.trim();
            labsString = labsString.replace(" ", "");
            labs = (ArrayList<Integer>) Stream.of(labsString.split(","))
                    .mapToInt(string -> Integer.parseInt(string))
                    .boxed()
                    .collect(Collectors.toList());
        } catch (Exception e) {
            activity.toast("parse error");
            return null;
        }

        return (ArrayList<Integer>) labs.stream().distinct().collect(Collectors.toList());
    }

    private void reserveWithoutPriority(ArrayList<Integer> labs) {
        String uid = mAuth.getUid();
        FirebaseDatabase.getInstance()
                .getReference()
                .child(Documents.Users)
                .child(uid).addListenerForSingleValueEvent(new ValueEventListener() {
                    @Override
                    public void onDataChange(@NonNull DataSnapshot dataSnapshot) {
                        UserInfo userInfo = dataSnapshot.getValue(UserInfo.class);
                        Order order = binding.getOrder();
                        int maxPriority = 0;

                        if (order.getQueue() != null) {

                            Optional<Map.Entry<String, Student>> queue = order.getQueue()
                                    .entrySet()
                                    .stream()
                                    .max((Map.Entry<String, Student> e1, Map.Entry<String, Student> e2) ->
                                            e1.getValue().getPriority() - e2.getValue().getPriority()
                                    );

                            if (queue.isPresent()) {
                                maxPriority = queue.get().getValue().getPriority();
                            }
                        }
                        Student student = new Student(++maxPriority,
                                labs,
                                userInfo.getName() + " " + userInfo.getSurname());
                        database.child("queue").child(uid).setValue(student);
                    }

                    @Override
                    public void onCancelled(@NonNull DatabaseError databaseError) {

                    }
                });
    }

    private void reserveWithPriority(ArrayList<Integer> labs) {
        String uid = mAuth.getUid();
        FirebaseDatabase.getInstance()
                .getReference()
                .child(Documents.Users)
                .child(uid).addListenerForSingleValueEvent(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot dataSnapshot) {
                UserInfo userInfo = dataSnapshot.getValue(UserInfo.class);
                Order order = binding.getOrder();
                if (order == null) return;

                if (order.getQueue() == null) order.setQueue(new HashMap<>());

                if (order.getFinished() == null) {
                    order.setFinished(new HashMap<>());
                }

                int priority = 0;

                if (labs.size() > 1) {
                    priority++;
                }

                if (!labs.contains(order.getCurrentLab())) {
                    priority++;
                }

                if (order.getFinished().containsKey(uid)) {
                    priority++;
                }

                Student student = new Student(priority,
                        labs,
                        userInfo.getName() + " " + userInfo.getSurname());
                database.child("queue").child(uid).setValue(student);
            }

            @Override
            public void onCancelled(@NonNull DatabaseError databaseError) {

            }
        });
    }

    private void configureDatabase() {
        MainActivity activity = (MainActivity) getActivity();
        database.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot dataSnapshot) {
                if (dataSnapshot.exists()) {
                    Order order = dataSnapshot.getValue(Order.class);
                    binding.setOrder(order);
                    configureButtons();
                } else {
                    activity.replaceFragment(RoomsFragment.class, BackStack.Default);
                }
            }

            @Override
            public void onCancelled(@NonNull DatabaseError databaseError) {

            }
        });

        database.child("queue").addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot dataSnapshot) {

                students = new ArrayList<>();
                for(DataSnapshot ds : dataSnapshot.getChildren()) {
                    String nameAndSurname = ds.child("nameAndSurname").getValue(String.class);
                    GenericTypeIndicator<ArrayList<Integer>> indicator = new GenericTypeIndicator<ArrayList<Integer>>() {};
                    ArrayList<Integer> labs = ds.child("labs").getValue(indicator);
                    int priority = ds.child("priority").getValue(int.class);
                    students.add(new Student(priority, labs, nameAndSurname));
                }
                Collections.sort(students, (Student s1, Student s2) -> s1.getPriority() - s2.getPriority());
                adapter.setStudents(students);
                adapter.notifyDataSetChanged();
            }

            @Override
            public void onCancelled(@NonNull DatabaseError databaseError) {

            }
        });
    }

    private void setupRecyclerView() {
        RecyclerView recyclerView = binding.students;
        LinearLayoutManager layoutManager = new LinearLayoutManager(getContext());
        recyclerView.setLayoutManager(layoutManager);
        adapter = new StudentAdapter(students, item -> {});
        recyclerView.setAdapter(adapter);
    }
}
