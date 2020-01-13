package com.laborder.ui.fragments;

import androidx.databinding.DataBindingUtil;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.firebase.auth.FirebaseAuth;
import com.laborder.R;
import com.laborder.bl.models.User;
import com.laborder.databinding.LoginFragmentBinding;

public class LoginFragment extends Fragment {
    private FirebaseAuth mAuth;
    private LoginFragmentBinding binding;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        mAuth = FirebaseAuth.getInstance();
        binding = DataBindingUtil.inflate(inflater, R.layout.login_fragment, container, false);
        return inflater.inflate(R.layout.login_fragment, container, false);
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
    }

    private void bindButtons() {

    }

    public void login(View view) {
        User user = binding.getUser();
    }

}
