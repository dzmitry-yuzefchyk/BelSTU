package com.laborder.ui.fragments;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;

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
import com.laborder.bl.Documents;
import com.laborder.bl.models.User;
import com.laborder.bl.models.UserInfo;
import com.laborder.databinding.RegisterFragmentBinding;

public class RegisterFragment extends Fragment {
    private FirebaseAuth mAuth;
    private RegisterFragmentBinding binding;
    DatabaseReference database;

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        mAuth = FirebaseAuth.getInstance();
        database = FirebaseDatabase.getInstance().getReference();
        binding = DataBindingUtil.inflate(inflater, R.layout.register_fragment, container, false);
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
        getView().findViewById(R.id.register_btn).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                register(v);
            }
        });
    }

    private void register(View view) {
        final User user = binding.getUser();
        final MainActivity activity = (MainActivity) getActivity();
        if (user.getEmail() == null || user.getName() == null
        || user.getPassword() == null || user.getSurname() == null) {
            activity.toast(getResources().getString(R.string.fill_all_fields));
            return;
        }

        mAuth.createUserWithEmailAndPassword(user.getEmail(), user.getPassword())
                .addOnCompleteListener(activity, new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            FirebaseUser currentUser = mAuth.getCurrentUser();
                            UserInfo userInfo = new UserInfo(user.getEmail(), user.getName(), user.getSurname());
                            database.child(Documents.Users).child(currentUser.getUid()).setValue(userInfo);
                            activity.toast(getResources().getString(R.string.register_success));
                            activity.replaceFragment(RoomsFragment.class, BackStack.Default);
                        } else {
                            activity.toast(getResources().getString(R.string.register_fail));
                        }
                    }
                });
    }
}
