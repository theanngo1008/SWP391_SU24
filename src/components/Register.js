import React, { useState } from 'react';
import { IoCloseOutline } from "react-icons/io5";
import { MdOutlineMailOutline } from "react-icons/md";
import { MdDriveFileRenameOutline } from "react-icons/md";
import { FaAddressBook, FaAddressCard, FaPhone } from "react-icons/fa6";
import { FaLock } from "react-icons/fa";
import axios from 'axios';

function Register(props) {
    const [email, setEmail] = useState('');
    const [fullName, setFullName] = useState('');
    const [numberPhone, setNumberPhone] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [address, setAddress] = useState('');
    const [errors, setErrors] = useState({});
    const API_URL = "http://localhost:5229/";

    const handleRegister = async (e) => {
        e.preventDefault();

        if (password !== confirmPassword) {
            alert("Password do not match!!!");
            return;
        }

        try {
            const response = await axios.post(API_URL + "api/Account/Register", {
                email: email,
                fullName: fullName,
                numberPhone: numberPhone,
                password: password,
                address: address
            }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            console.log(response.data);
            alert("Registration successful");
            props.toggle();
        } catch (error) {
            console.error('There was an error!!!', error.response);
            if (error.response && error.response.data && error.response.data.errors) {
                setErrors(error.response.data.errors);
                console.log(error.response.data.errors);
            } else {
                alert('Registration failed!!!');
            }
        }
    };

    return (
        <div className="popup">
            <div className="popup-inner">
                <h2>Register</h2>
                <span className="popup-close" onClick={props.toggle}><IoCloseOutline /></span>

                <form onSubmit={handleRegister}>
                    <label>
                        <MdOutlineMailOutline /> Email
                        <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
                        {errors.email && <div className="error">{errors.email.join(', ')}</div>}
                    </label>
                    <label>
                        <MdDriveFileRenameOutline /> Full Name
                        <input type="text" value={fullName} onChange={e => setFullName(e.target.value)} required />
                        {errors.fullName && <div className="error">{errors.fullName.join(', ')}</div>}
                    </label>
                    <label>
                        <FaPhone /> Phone Number
                        <input type="text" value={numberPhone} onChange={e => setNumberPhone(e.target.value)} required />
                        {errors.NumberPhone && <div className="error">{errors.NumberPhone.join(', ')}</div>}
                    </label>
                    <label>
                        <FaLock /> Password
                        <input type="password" value={password} onChange={e => setPassword(e.target.value)} required />
                        {errors.Password && <div className="error">{errors.Password.join(', ')}</div>}
                    </label>
                    <label>
                        <FaLock /> Confirm password
                        <input type="password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)} required />
                    </label>
                    <label>
                        <FaAddressCard /> Address
                        <input type="text" value={address} onChange={e => setAddress(e.target.value)} />
                        {errors.Address && <div className="error">{errors.Address.join(', ')}</div>}
                    </label>
                    <button type="submit">Register</button>
                </form>

            </div>
        </div>
    );
}

export default Register;