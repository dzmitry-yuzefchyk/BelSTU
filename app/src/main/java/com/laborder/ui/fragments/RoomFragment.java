package com.laborder.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.database.ChildEventListener;
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
import com.laborder.bl.adapters.OrderInfoAdapter;
import com.laborder.bl.adapters.StudentAdapter;
import com.laborder.bl.models.Order;
import com.laborder.bl.models.OrderInfo;
import com.laborder.bl.models.Student;
import com.laborder.bl.models.StudentInfo;
import com.laborder.databinding.RoomFragmentBinding;
import com.laborder.databinding.RoomsFragmentBinding;

import java.util.ArrayList;
import java.util.List;

public class RoomFragment extends Fragment {
    private String id;
    private RoomFragmentBinding binding;
    private List<StudentInfo> students;
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
    }

    private void configureButtons() {
        if (binding.getOrder().getCreatorId().equals(mAuth.getUid())) {
            getView().findViewById(R.id.next_btn).setVisibility(View.VISIBLE);
        } else {
            getView().findViewById(R.id.reserve_btn).setVisibility(View.VISIBLE);
        }
    }

    private void reserve() {
        MainActivity activity = (MainActivity) getActivity();
        if (userAlreadyInOrder()) {
            activity.toast("to reserve bla bla");
            return;
        }

        if (binding.getOrder().isUsePriority()) {

        } else {
            reserveWithoutPriority();
        }
    }

    private void reserveWithoutPriority() {

    }

    private boolean userAlreadyInOrder() {
        final boolean[] isInOrder = new boolean[1];
        String uid = mAuth.getUid();
        database.child("queue").child(uid).addListenerForSingleValueEvent(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot dataSnapshot) {
                if (dataSnapshot.getValue() != null) {
                    isInOrder[0] = true;
                } else {
                    isInOrder[0] = false;
                }
            }

            @Override
            public void onCancelled(@NonNull DatabaseError databaseError) {

            }
        });

        return isInOrder[0];
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
                if (!dataSnapshot.exists()) {
                    return;
                }

                students = new ArrayList<>();
                for(DataSnapshot ds : dataSnapshot.getChildren()) {
                    String uid = ds.getKey();
                    String nameAndSurname = ds.child("nameAndSurname").getValue(String.class);
                    GenericTypeIndicator<ArrayList<Integer>> indicator = new GenericTypeIndicator<ArrayList<Integer>>() {};
                    ArrayList<Integer> labs = ds.child("labs").getValue(indicator);
                    int labsCount = labs.size();
                    students.add(new StudentInfo(uid, nameAndSurname, labsCount));
                }
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
