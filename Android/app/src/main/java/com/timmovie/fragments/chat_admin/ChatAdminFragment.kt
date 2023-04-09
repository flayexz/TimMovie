package com.timmovie.fragments.chat_admin

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.timmovie.R

class ChatAdminFragment : Fragment() {

    companion object {
        fun newInstance() = ChatAdminFragment()
    }

    private lateinit var viewModel: ChatAdminViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_chat_admin, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        viewModel = ViewModelProvider(this).get(ChatAdminViewModel::class.java)
        // TODO: Use the ViewModel
    }

}