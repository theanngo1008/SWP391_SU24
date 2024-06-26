import React, { useState, useEffect } from 'react';
import { IoCloseOutline } from "react-icons/io5";
import { MdOutlineMailOutline } from "react-icons/md";
import { FaLock } from "react-icons/fa";
import { RiLoginBoxLine } from "react-icons/ri";
import ForgotPassword from 'pages/users/forgotPasswordPage/ForgotPassword';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function Login({ toggle, setUser, setAccName }) {
    const navigate = useNavigate();
    const [showForgotPassword, setShowForgotPassword] = useState(false);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const API_URL = "http://localhost:5229/";

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(API_URL + 'api/Account/Login', {
                email: email,
                password: password,
            });

            if (response.data) {
                const token = response.data.token;
                const account = response.data.account;

                // Lưu token và thông tin tài khoản vào localStorage
                localStorage.setItem('token', token);
                localStorage.setItem('account', JSON.stringify(account)); // Chuyển đổi đối tượng thành chuỗi JSON

                console.log("Logged in account:", account); // Thêm log để kiểm tra

                if (rememberMe) {
                    localStorage.setItem('email', email);
                    localStorage.setItem('password', password);
                } else {
                    localStorage.removeItem('email');
                    localStorage.removeItem('password');
                }

                setUser(email);
                setAccName(account.accName);

                // Kiểm tra vai trò của người dùng và chuyển hướng
                if (account.role === 'MN') {
                    navigate('/manager/');
                } else if (account.role === 'AD') {
                    navigate('/admin/');
                } else {
                    navigate('/'); // Chuyển hướng đến trang chat sau khi đăng nhập thành công
                }

                toggle(); // Đóng popup
            }
        } catch (error) {
            console.error('Có lỗi xảy ra:', error);
            alert('Đăng nhập thất bại!');
        }
    };

    const handleForgotPassword = () => {
        setShowForgotPassword(true);
    };

    useEffect(() => {
        // Xóa tất cả các mục trong localStorage khi khởi động dự án
        localStorage.clear();

        const savedEmail = localStorage.getItem('email');
        const savedPassword = localStorage.getItem('password');
        const token = localStorage.getItem('token');

        console.log('Saved Email:', savedEmail);
        console.log('Saved Password:', savedPassword);
        console.log('Token:', token);

        if (savedEmail && savedPassword) {
            setEmail(savedEmail);
            setPassword(savedPassword);
            setRememberMe(true);
        }

        if (token) {
            const account = localStorage.getItem('account');
            if (account) {
                try {
                    const parsedAccount = JSON.parse(account);
                    console.log('Parsed Account:', parsedAccount);
                    setUser(parsedAccount.email);
                    setAccName(parsedAccount.accName);
                } catch (error) {
                    console.error('Failed to parse account from localStorage', error);
                }
            }
        }
    }, [navigate, setUser, setAccName]);

    return (
        <div className="popup">
            <div className="popup-inner">
                <h2>{showForgotPassword ? "" : "Login"}</h2>
                <span className="popup-close" onClick={toggle}><IoCloseOutline /></span>
                {!showForgotPassword && (
                    <form onSubmit={handleSubmit}>
                        <label>
                            <MdOutlineMailOutline /> Email
                            <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} />
                        </label>
                        <label>
                            <FaLock /> Password
                            <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                        </label>

                        <div className="remember-me">
                            <input
                                type="checkbox"
                                checked={rememberMe}
                                onChange={(e) => setRememberMe(e.target.checked)}
                            />
                            Remember Me
                        </div>

                        <button type="submit"><RiLoginBoxLine /> LOGIN</button>
                    </form>
                )}
                {showForgotPassword && (
                    <ForgotPassword setShowForgotPassword={setShowForgotPassword} />
                )}
                {!showForgotPassword && (
                    <button className="forgot-password" onClick={handleForgotPassword} role="link" aria-label="Forgot your password?">
                        Forgot your password?
                    </button>
                )}
            </div>
        </div>
    );
}

export default Login;
