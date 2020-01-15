package com.laborder.bl.models;

import java.util.ArrayList;

public class StudentInfo {
    private String id;
    private String nameAndSurname;
    private int labsCount;

    public int getLabsCount() {
        return labsCount;
    }

    public void setLabsCount(int labsCount) {
        this.labsCount = labsCount;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getNameAndSurname() {
        return nameAndSurname;
    }

    public void setNameAndSurname(String nameAndSurname) {
        this.nameAndSurname = nameAndSurname;
    }

    public StudentInfo(String id, String nameAndSurname, int labsCount) {
        this.id = id;
        this.nameAndSurname = nameAndSurname;
        this.labsCount = labsCount;
    }

}
