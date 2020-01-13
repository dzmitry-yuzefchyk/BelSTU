package com.laborder;

import android.os.Bundle;
import android.os.Handler;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;

import com.google.firebase.auth.FirebaseAuth;
import com.laborder.bl.BackStack;
import com.laborder.ui.fragments.MainFragment;
import com.laborder.ui.fragments.RoomsFragment;

public class MainActivity extends AppCompatActivity {
    private FirebaseAuth auth;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main_activity);
        checkAuth();
    }

    private void checkAuth() {
        auth = FirebaseAuth.getInstance();

        if (auth.getCurrentUser() != null) {
            replaceFragment(RoomsFragment.class, BackStack.Default);
        } else {
            replaceFragment(MainFragment.class, BackStack.Null);
        }
    }

    public void toast(String text) {
        int duration = Toast.LENGTH_SHORT;
        Toast toast = Toast.makeText(getApplicationContext(), text, duration);
        toast.show();
    }

    public void replaceFragment(Class fragmentClass, String backStack) {
        Fragment fragment = null;
        try {
            fragment = (Fragment) fragmentClass.newInstance();
        } catch (Exception e) {
            e.printStackTrace();
        }

        replaceFragment(fragment, backStack);
    }

    public void replaceFragment(Class fragmentClass, String backStack, Bundle args) {
        Fragment fragment = null;
        try {
            fragment = (Fragment) fragmentClass.newInstance();
            fragment.setArguments(args);
        } catch (Exception e) {
            e.printStackTrace();
        }

        replaceFragment(fragment, backStack);
    }

    private void replaceFragment(final Fragment fragment, final String backStack) {
        Handler handler = new Handler();
        handler.post(new Runnable() {
            @Override
            public void run() {
                FragmentTransaction transaction = getSupportFragmentManager()
                        .beginTransaction();
                transaction.replace(R.id.container, fragment);

                if (backStack != null) {
                    transaction.addToBackStack(backStack);
                }

                transaction.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN)
                        .commit();
            }
        });
    }
}
