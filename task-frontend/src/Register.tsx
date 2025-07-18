import { useState } from "react";
import React from "react";
import ApiService from './helpers/ApiService.ts';
import './styles/register.css';
import { useNavigate } from "react-router-dom";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

import { deAT } from "date-fns/locale";

export default function Register() {

    const navigate = useNavigate();
    interface User {
        user_name: string;
        user_surname: string;
        user_email: string;
        user_birthdate: string;
        user_password: string;
    }

    const [newUser, setNewUser] = useState<User>({
        user_name: "",
        user_surname: "",
        user_email: "",
        user_birthdate: "",
        user_password: ""
    })

    const [validEmail, setValidEmail] = useState(true);

    function hasOneSymbol(email: string): boolean {
        return email.split('@').length === 2;
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setNewUser((prev) => ({ ...prev, [name]: value }));
    };

    const handleRegister = async (user_name: string, user_surname: string, user_email: string, user_birthdate: string, user_password: string): Promise<boolean> => {
        try {
            if(hasOneSymbol(user_email)){
                const result = ApiService.register(user_name, user_surname, user_email, user_birthdate, user_password).then(() => setNewUser({
                    user_name: "",
                    user_surname: "",
                    user_email: "",
                    user_birthdate: "",
                    user_password: ""
                })).then(() => alert("User registered")).then(() => navigate('/'));
                setValidEmail(true);
                return true;
            } else {
                setValidEmail(false);
                return false;
            }
            
        } catch (error) {
            console.error(error);
            return false;
        }
    }

    return (
        <>
            <form className="login-form">
                <h2 className="login-header">Register</h2>

                <div className="login-div">
                    <label htmlFor="name" className="login-label">Name:</label>
                    <input
                        type="user_name"
                        name="user_name"
                        id="user_name"
                        value={newUser.user_name}
                        onChange={handleChange}
                        required
                        className="login-input"
                    />
                </div>

                <div className="login-div">
                    <label htmlFor="surname" className="login-label">Surname:</label>
                    <input
                        type="user_surname"
                        name="user_surname"
                        id="user_surname"
                        value={newUser.user_surname}
                        onChange={handleChange}
                        required
                        className="login-input"
                    />
                </div>
                <div className="login-div">
                    <label htmlFor="email" className="login-label">Email:</label>
                    <input
                        type="user_email"
                        name="user_email"
                        id="user_email"
                        value={newUser.user_email}
                        onChange={handleChange}
                        required
                        className="login-input"
                    />
                </div>

                <div className="login-div">
                    <label htmlFor="password" className="login-label">Password:</label>
                    <input
                        type="user_password"
                        name="user_password"
                        id="user_password"
                        value={newUser.user_password}
                        onChange={handleChange}
                        required
                        className="login-input"
                    />
                </div>

                <label>Birthdate:</label>
                <DatePicker
                    selected={newUser.user_birthdate ? new Date(newUser.user_birthdate) : null}
                    onChange={date => {
                        if (date) {
                            // Format as YYYY-MM-DDTHH:mm:00
                            const year = date.getFullYear();
                            const month = String(date.getMonth() + 1).padStart(2, '0');
                            const day = String(date.getDate()).padStart(2, '0');
                            const formatted = `${year}-${month}-${day}`;
                            setNewUser(prev => ({
                                ...prev,
                                user_birthdate: formatted
                            }));
                        } else {
                            setNewUser(prev => ({
                                ...prev,
                                user_birthdate: ""
                            }));
                        }
                    }}
                    dateFormat="yyyy-MM-dd"
                    placeholderText="Select Birthdate"
                />

                <button
                    type="button"
                    className="login-button"
                    onClick={() => handleRegister(newUser.user_name, newUser.user_surname, newUser.user_email, newUser.user_birthdate, newUser.user_password)}
                >

                    Register
                </button>
                {
                    !validEmail && <p className="email-alert">Email is not valid</p>
                }
            </form>

        </>
    );
}