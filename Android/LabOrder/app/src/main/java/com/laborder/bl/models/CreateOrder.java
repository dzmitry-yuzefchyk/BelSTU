package com.laborder.bl.models;

import androidx.databinding.BaseObservable;
import androidx.databinding.Bindable;
import androidx.databinding.library.baseAdapters.BR;

public class CreateOrder extends BaseObservable {

    private String title;
    private int currentLab;
    private boolean usePriority;

    @Bindable
    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
        notifyPropertyChanged(BR.title);
    }

    @Bindable
    public int getCurrentLab() {
        return currentLab;
    }

    public void setCurrentLab(int currentLab) {
        this.currentLab = currentLab;
        notifyPropertyChanged(BR.currentLab);
    }

    @Bindable
    public boolean getUsePriority() {
        return usePriority;
    }

    public void setUsePriority(boolean usePriority) {
        this.usePriority = usePriority;
        notifyPropertyChanged(BR.usePriority);
    }
}
