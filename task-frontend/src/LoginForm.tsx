import { useState } from "react";
import React from "react";
import ApiService from './helpers/ApiService.ts';
import './styles/loginform.css';
import { useNavigate } from "react-router-dom";;

function LoginForm(){



    type LoginFormData = {
        email: string;
        password: string;
    }

    const navigate = useNavigate();
    const [validLogini ,setValidLogin] = useState(true);

    const [userData, setUserData] = useState<LoginFormData>(
        {email: '',
        password: ''}
    );

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setUserData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        console.log("Login Data:", userData);

        ApiService.login(userData.email, userData.password)
        .then((res)=>{
          console.log("Login succesfull" , res);
          setUserData({email:"", password:""});
          if(res == true){
            navigate('/home');
            setValidLogin(true);
          } else {
            setValidLogin(false);
          }

        })
        .catch((err) => {
          console.error("Couldnt login:" , err);

        })
    
        
      };

    return (
        <form onSubmit={handleSubmit} className="login-form">
        <h2 className="login-header">Login</h2>
  
        <div className="login-div">
          <label htmlFor="email" className="login-label">Email:</label>
          <input
            type="email"
            name="email"
            id="email"
            value={userData.email}
            onChange={handleChange}
            required
            className="login-input"
          />
        </div>
  
        <div className="login-div">
          <label htmlFor="password" className="login-label">Password:</label>
          <input
            type="password"
            name="password"
            id="password"
            value={userData.password}
            onChange={handleChange}
            required
            className="login-input"
          />
        </div>
  
        <button
          type="button"
          className="login-button"
          onClick={handleSubmit}
        >
        
          Login
        </button>
        <button
          type="button"
          className="register-button"
          onClick={() => navigate('/register')}
        >
          Register
        </button>
        {!validLogini &&
        <p className="login-alert">Login Failed!</p>
        }
        
      </form>
    )
}

export default LoginForm;