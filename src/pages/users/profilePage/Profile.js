import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Profile.scss';

function Profile({ user, setUser }) {
    const [formData, setFormData] = useState({
        accName: '',
        email: '',
        phone: '',
        address: '',
        password: ''
    });
    const [message, setMessage] = useState('');

    useEffect(() => {
        // Fetch user details
        const fetchUserDetails = async () => {
            try {
                const response = await axios.get('http://localhost:5229/api/Account/UserDetails', {
                    headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
                });
                const userDetails = response.data;
                setFormData({

                    email: userDetails.email,
                    accName: userDetails.accName,
                    phone: userDetails.phone,
                    address: userDetails.address,
                    password: ''
                });
            } catch (error) {
                console.error('Error fetching user details', error);
            }
        };

        fetchUserDetails();
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.put('http://localhost:5229/api/Account/UpdateProfile', formData, {
                headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
            });
            setMessage('Profile updated successfully!');
            setUser(formData.email); // Update user email in parent component
        } catch (error) {
            console.error('Error updating profile', error);
            setMessage('Failed to update profile');
        }
    };

    return (
        <div className="profile">
            <h2>Edit Profile</h2>
            <form onSubmit={handleSubmit}>

                <label>
                    Email
                    <input
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                    />
                </label>
                <label>
                    Account Name
                    <input
                        type="text"
                        name="accName"
                        value={formData.accName}
                        onChange={handleChange}
                    />
                </label>
                <label>
                    Phone Number
                    <input
                        type="tel"
                        name="phone"
                        value={formData.phone}
                        onChange={handleChange}
                    />
                </label>
                <label>
                    Address
                    <input
                        type="text"
                        name="address"
                        value={formData.address}
                        onChange={handleChange}
                    />
                </label>
                <label>
                    Password
                    <input
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                    />
                </label>
                <button type="submit">Save</button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
}

export default Profile;
