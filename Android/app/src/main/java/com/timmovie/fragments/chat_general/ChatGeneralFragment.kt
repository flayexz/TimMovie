package com.timmovie.fragments.chat_general

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.timmovie.R

class ChatGeneralFragment : Fragment() {

    companion object {
        fun newInstance() = ChatGeneralFragment()
    }

    private lateinit var viewModel: ChatGeneralViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_chat_general, container, false)
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        viewModel = ViewModelProvider(this).get(ChatGeneralViewModel::class.java)
        // TODO: Use the ViewModel
    }

}