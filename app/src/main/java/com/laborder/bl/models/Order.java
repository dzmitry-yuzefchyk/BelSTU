package com.laborder.bl.models;

import java.util.ArrayList;
import java.util.Queue;

public class Order {
    public String Id;
    public String CreatorId;
    public boolean UsePriority;
    public int CurrentLab;
    public ArrayList<Student> Queue;
    public Queue<Student> Finished;
    public Student Current;
}
