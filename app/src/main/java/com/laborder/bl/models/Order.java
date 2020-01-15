package com.laborder.bl.models;

import java.util.ArrayList;

public class Order {
    private String creatorId;
    private String title;
    private boolean usePriority;
    private int currentLab;
    private ArrayList<Student> queue;
    private ArrayList<Student> finished;
    private Student currentStudent;

    public String getCreatorId() {
        return creatorId;
    }

    public void setCreatorId(String creatorId) {
        this.creatorId = creatorId;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public boolean isUsePriority() {
        return usePriority;
    }

    public void setUsePriority(boolean usePriority) {
        this.usePriority = usePriority;
    }

    public int getCurrentLab() {
        return currentLab;
    }

    public void setCurrentLab(int currentLab) {
        this.currentLab = currentLab;
    }

    public ArrayList<Student> getQueue() {
        return queue;
    }

    public void setQueue(ArrayList<Student> queue) {
        this.queue = queue;
    }

    public ArrayList<Student> getFinished() {
        return finished;
    }

    public void setFinished(ArrayList<Student> finished) {
        this.finished = finished;
    }

    public Student getCurrentStudent() {
        return currentStudent;
    }

    public void setCurrentStudent(Student currentStudent) {
        this.currentStudent = currentStudent;
    }

    public Order(String creatorId, String title, boolean usePriority, int currentLab,
                 ArrayList<Student> queue, ArrayList<Student> finished, Student currentStudent) {
        this.creatorId = creatorId;
        this.title = title;
        this.usePriority = usePriority;
        this.currentLab = currentLab;
        this.queue = queue;
        this.finished = finished;
        this.currentStudent = currentStudent;
    }
}
