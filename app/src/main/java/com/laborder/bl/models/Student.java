package com.laborder.bl.models;

import java.util.ArrayList;

public class Student {
    private String id;
    private int priority;
    private ArrayList<Integer> labs;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public int getPriority() {
        return priority;
    }

    public void setPriority(int priority) {
        this.priority = priority;
    }

    public ArrayList<Integer> getLabs() {
        return labs;
    }

    public void setLabs(ArrayList<Integer> labs) {
        this.labs = labs;
    }

    public Student() {}

    public Student(String id, int priority, ArrayList<Integer> labs) {
        this.id = id;
        this.priority = priority;
        this.labs = labs;
    }
}
