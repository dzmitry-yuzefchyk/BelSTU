package com.laborder.ui.fragments;

import androidx.databinding.DataBindingUtil;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;
import com.laborder.MainActivity;
import com.laborder.R;
import com.laborder.bl.BackStack;
import com.laborder.bl.models.User;
import com.laborder.databinding.LoginFragmentBinding;

public class LoginFragment extends Fragment {
    private FirebaseAuth mAuth;
    private LoginFragmentBinding binding;
    DatabaseReference database;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        mAuth = FirebaseAuth.getInstance();
        database = FirebaseDatabase.getInstance().getReference();
        binding = DataBindingUtil.inflate(inflater, R.layout.login_fragment, container, false);
        binding.setUser(new User());
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
        getView().findViewById(R.id.login_btn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                login(v);
            }
        });
    }

    public void login(View view) {
        final User user = binding.getUser();
        final MainActivity activity = (MainActivity) getActivity();
        if (user.getEmail() == null || user.getPassword() == null) {
            activity.toast(getResources().getString(R.string.fill_all_fields));
            return;
        }

        mAuth.signInWithEmailAndPassword(user.getEmail(), user.getPassword())
                .addOnCompleteListener(getActivity(), new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            activity.handleDrawer();
                            activity.replaceFragment(RoomsFragment.class, BackStack.Default);
                        } else {
                            activity.toast(getResources().getString(R.string.invalid_email_or_password));
                        }
                    }
                });
    }

}
