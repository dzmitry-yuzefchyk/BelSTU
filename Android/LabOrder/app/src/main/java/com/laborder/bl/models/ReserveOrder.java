package com.laborder.bl.models;

import androidx.databinding.BaseObservable;
import androidx.databinding.Bindable;
import androidx.databinding.library.baseAdapters.BR;

public class ReserveOrder extends BaseObservable {
    private String labs;

    @Bindable
    public String getLabs() {
        return labs;
    }

    public void setLabs(String labs) {
        this.labs = labs;
        notifyPropertyChanged(BR.labs);
    }

    public ReserveOrder(String labs) {
        this.labs = labs;
    }

    public ReserveOrder() {}
}
