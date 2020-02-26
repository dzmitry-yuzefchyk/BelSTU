package com.laborder.bl.models;

import java.util.HashMap;

public class Order {
    private String creatorId;
    private String title;
    private boolean usePriority;
    private int currentLab;
    private HashMap<String, Student> queue;
    private HashMap<String, Student> finished;
    private Student current;

    public Student getCurrent() {
        return current;
    }

    public void setCurrent(Student current) {
        this.current = current;
    }

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

    public HashMap<String, Student> getQueue() {
        return queue;
    }

    public void setQueue(HashMap<String, Student> queue) {
        this.queue = queue;
    }

    public HashMap<String, Student> getFinished() {
        return finished;
    }

    public void setFinished(HashMap<String, Student> finished) {
        this.finished = finished;
    }


    public Order(String creatorId, String title, boolean usePriority, int currentLab,
                 HashMap<String, Student> queue, HashMap<String, Student> finished, Student current) {
        this.creatorId = creatorId;
        this.title = title;
        this.usePriority = usePriority;
        this.currentLab = currentLab;
        this.queue = queue;
        this.finished = finished;
        this.current = current;
    }

    public Order() {}
}
