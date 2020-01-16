package com.laborder.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;

import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;
import com.laborder.MainActivity;
import com.laborder.R;
import com.laborder.bl.BackStack;
import com.laborder.bl.Documents;
import com.laborder.bl.models.CreateOrder;
import com.laborder.bl.models.Order;
import com.laborder.bl.models.Student;
import com.laborder.databinding.CreateRoomFragmentBinding;

import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.HashMap;

public class CreateRoomFragment extends Fragment {
    private CreateRoomFragmentBinding binding;
    private FirebaseAuth mAuth;
    DatabaseReference database;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        mAuth = FirebaseAuth.getInstance();
        database = FirebaseDatabase.getInstance().getReference();
        binding = DataBindingUtil.inflate(inflater, R.layout.create_room_fragment, container, false);
        binding.setOrder(new CreateOrder());
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
        getView().findViewById(R.id.create_btn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                createOrder(v);
            }
        });
    }

    private void createOrder(View v) {
        final CreateOrder baseOrderData = binding.getOrder();
        final MainActivity activity = (MainActivity) getActivity();
        if (baseOrderData.getTitle() == null) {
            activity.toast(getResources().getString(R.string.fill_all_fields));
            return;
        }

        FirebaseUser user = mAuth.getCurrentUser();
        String key = database.push().getKey();
        Order order = new Order(
                user.getUid(),
                baseOrderData.getTitle(),
                baseOrderData.getUsePriority(),
                baseOrderData.getCurrentLab(),
                new HashMap<>(),
                new HashMap<>());

        database.child(Documents.Orders).child(key).setValue(order);
        database.child(Documents.OrdersIds).child(key).setValue(baseOrderData.getTitle());

        Bundle data = new Bundle();
        data.putString("id", key);
        activity.replaceFragment(RoomFragment.class, BackStack.Default, data);
    }
}
