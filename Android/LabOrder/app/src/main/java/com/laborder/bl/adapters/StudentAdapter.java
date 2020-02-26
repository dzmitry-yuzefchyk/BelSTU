package com.laborder.bl.adapters;

import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.laborder.bl.models.Student;
import com.laborder.databinding.StudentInfoListRowBinding;

import java.util.List;

public class StudentAdapter extends RecyclerView.Adapter<StudentAdapter.StudentViewHolder> {

    public interface OnItemClickListener {
        void onItemClick(Student item);
    }

    private final OnItemClickListener listener;
    private List<Student> students;

    public StudentAdapter(List<Student> students, OnItemClickListener listener) {
        this.students = students;
        this.listener = listener;
    }

    @NonNull
    @Override
    public StudentViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(parent.getContext());
        StudentInfoListRowBinding itemBinding = StudentInfoListRowBinding.inflate(layoutInflater, parent, false);
        return new StudentViewHolder(itemBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull StudentViewHolder holder, int position) {
        Student student = students.get(position);
        holder.bind(student, listener);
    }

    @Override
    public int getItemCount() {
        return students != null ? students.size() : 0;
    }

    public void setStudents(List<Student> students) {
        this.students = students;
    }


    class StudentViewHolder extends RecyclerView.ViewHolder {
        private StudentInfoListRowBinding binding;

        public StudentViewHolder(StudentInfoListRowBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }

        public void bind(Student student, final OnItemClickListener listener) {
            binding.setStudent(student);
            binding.executePendingBindings();
            itemView.setOnClickListener(v -> listener.onItemClick(student));
        }
    }
}
