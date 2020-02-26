package com.laborder.bl.models;

import java.util.ArrayList;

public class Student {
    private String nameAndSurname;
    private int priority;
    private ArrayList<Integer> labs;
    private int labsCount;

    public int getLabsCount() {
        return labsCount;
    }

    public void setLabsCount(int labsCount) {
        this.labsCount = labsCount;
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

    public String getNameAndSurname() {
        return nameAndSurname;
    }

    public void setNameAndSurname(String nameAndSurname) {
        this.nameAndSurname = nameAndSurname;
    }

    public Student() {}

    public Student(int priority, ArrayList<Integer> labs, String nameAndSurname) {
        this.priority = priority;
        this.labs = labs;
        this.nameAndSurname = nameAndSurname;
        this.labsCount = labs.size();
    }
}
