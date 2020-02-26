package com.laborder.bl.adapters;

import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.laborder.bl.models.OrderInfo;
import com.laborder.databinding.OrderInfoListRowBinding;

import java.util.List;

public class OrderInfoAdapter extends RecyclerView.Adapter<OrderInfoAdapter.OrderViewHolder> {

    public interface OnItemClickListener {
        void onItemClick(OrderInfo item);
    }

    private final OnItemClickListener listener;
    private List<OrderInfo> orders;

    public OrderInfoAdapter(List<OrderInfo> ordersList, OnItemClickListener listener) {
        this.orders = ordersList;
        this.listener = listener;
    }

    @NonNull
    @Override
    public OrderViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater layoutInflater = LayoutInflater.from(parent.getContext());
        OrderInfoListRowBinding itemBinding = OrderInfoListRowBinding.inflate(layoutInflater, parent, false);
        return new OrderViewHolder(itemBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull OrderViewHolder holder, int position) {
        OrderInfo order = orders.get(position);
        holder.bind(order, listener);
    }

    @Override
    public int getItemCount() {
        return orders != null ? orders.size() : 0;
    }

    public void setOrders(List<OrderInfo> ordersList) {
        this.orders = ordersList;
    }


    class OrderViewHolder extends RecyclerView.ViewHolder {
        private OrderInfoListRowBinding binding;

        public OrderViewHolder(OrderInfoListRowBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }

        public void bind(OrderInfo orderInfo, final OnItemClickListener listener) {
            binding.setOrder(orderInfo);
            binding.executePendingBindings();
            itemView.setOnClickListener(v -> listener.onItemClick(orderInfo));
        }
    }
}
