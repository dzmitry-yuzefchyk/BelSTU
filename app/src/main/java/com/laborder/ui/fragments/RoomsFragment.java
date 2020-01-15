package com.laborder.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

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
import com.laborder.bl.adapters.OrderAdapter;
import com.laborder.bl.models.OrderInfo;
import com.laborder.databinding.RoomsFragmentBinding;

import java.util.ArrayList;
import java.util.List;

public class RoomsFragment extends Fragment {
    private DatabaseReference database;
    private RoomsFragmentBinding binding;
    private List<OrderInfo> orders;
    private OrderAdapter adapter;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {

        binding = DataBindingUtil.inflate(inflater, R.layout.rooms_fragment, container, false);
        database = FirebaseDatabase.getInstance().getReference().child(Documents.OrdersIds);
        orders = new ArrayList<>();
        setupRecyclerView();
        configureDatabase();
        return binding.getRoot();
    }

    private void configureDatabase() {
        database.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot dataSnapshot) {
                orders = new ArrayList<>();
                for(DataSnapshot ds : dataSnapshot.getChildren()) {
                    String uid = ds.getKey();
                    String title = ds.getValue(String.class);
                    orders.add(new OrderInfo(title, uid));
                }
                adapter.setOrders(orders);
                adapter.notifyDataSetChanged();
            }

            @Override
            public void onCancelled(@NonNull DatabaseError databaseError) {
            }
        });
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
        getView().findViewById(R.id.create_order).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openCreateRoomFragment(v);
            }
        });
    }

    private  void openCreateRoomFragment(View v) {
        MainActivity activity = (MainActivity) getActivity();
        activity.replaceFragment(CreateRoomFragment.class, BackStack.Default);
    }

    private void setupRecyclerView() {
        RecyclerView recyclerView = binding.allOrders;
        LinearLayoutManager layoutManager = new LinearLayoutManager(getContext());
        recyclerView.setLayoutManager(layoutManager);
        adapter = new OrderAdapter(orders, item -> openRoomFragment(item));
        recyclerView.setAdapter(adapter);
    }

    private void openRoomFragment(OrderInfo orderInfo) {
        MainActivity activity = (MainActivity) getActivity();
        Bundle data = new Bundle();
        data.putString("id", orderInfo.getId());
        activity.replaceFragment(RoomFragment.class, BackStack.Default, data);
    }
}
