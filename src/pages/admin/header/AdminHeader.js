import React from 'react';
import { Link } from 'react-router-dom';
import './AdminHeader.scss';

const AdminHeader = () => {
    return (
        <header className="admin-header">
            <div className="admin-header__logo">
                <Link to="/admin">Admin Dashboard</Link>
            </div>
            <nav className="admin-header__nav">
                <ul>
                    <li><Link to="/admin/manage-accounts">Quản lý người dùng</Link></li>
                    <li><Link to="/admin/settings">Cài đặt</Link></li>
                    <li><Link to="/logout">Đăng xuất</Link></li>
                </ul>
            </nav>
        </header>
    );
};

export default AdminHeader;
