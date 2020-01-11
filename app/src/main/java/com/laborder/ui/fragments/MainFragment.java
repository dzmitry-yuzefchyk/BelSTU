package com.laborder.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.laborder.MainActivity;
import com.laborder.R;
import com.laborder.bl.BackStack;

public class MainFragment extends Fragment {

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        return inflater.inflate(R.layout.main_fragment, container, false);
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
    }

    @Override
    public void onStart() {
        super.onStart();
        getView().findViewById(R.id.login_btn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                switchToLogin(v);
            }
        });

        getView().findViewById(R.id.register_btn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                switchToRegister(v);
            }
        });
    }

    private void switchToLogin(View view) {
        ((MainActivity) getActivity()).replaceFragment(LoginFragment.class, BackStack.Default);
    }

    private void switchToRegister(View view) {
        ((MainActivity) getActivity()).replaceFragment(RegisterFragment.class, BackStack.Default);
    }
}
