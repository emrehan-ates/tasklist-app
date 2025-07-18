import React from "react";
import ApiService from './helpers/ApiService.ts';
import { useNavigate, useLocation } from "react-router-dom";
import { useState, useEffect } from 'react';
import type { User } from './helpers/ApiService';
import ListPage from "./ListPage.tsx";
import useModal from "./helpers/useModal.tsx";
import './styles/taskspage.css'
import logo from './assets/icon.png'

export default function Profile() {


    const [isUpdate, setUpdate] = useState(false);
    // interface User{
    //     user_name:string;
    //     user_surname:string;
    //     user_email:string;
    //     user_birthdate:string;
    //     user_created:string;
    // }

    const [user, setUser] = useState<User>({
        user_id:0,
        user_name:"",
        user_surname:"",
        user_email:"",
        user_birthdate:"",
        user_created:"",
        user_password:""
    })


    const getProfile = () => {
        ApiService.getProfile().then(data => setUser(data)).catch(err => console.error(err));
        
    }
    getProfile();
    const navigate = useNavigate();

    const dateFormatter = async (date:string):Promise<String> => {

        const formatted = date.split("T");
        return formatted[0];
    }


    return (

        <>
        

        <div className="top-menu">
            <img src={logo} alt="logo" className="logo" onClick={() => navigate('/home')} ></img>
            <span  onClick={() => navigate('/home')} >Home</span>
            <span className="profile-button" onClick={() => navigate('/home')}>Profile</span>
        </div>
        

        <div className="profile-container">
            <label className="name-label">Name: </label>
            <label className="Name">{user.user_name}</label>
            <label className="name-label">Surname: </label>
            <label className="Name">{user.user_surname}</label>
            <label className="name-label">E-mail: </label>
            <label className="Name">{user.user_email}</label>
            <label className="name-label">Birthdate: </label>
            <label className="Name">{user.user_birthdate}</label>
            <label className="name-label">Date of join: </label>
            <label className="Name">{user.user_created}</label>


        </div>
        
        
        </>


    );
}