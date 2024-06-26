import React from 'react';
import './AdminFooter.scss';

const AdminFooter = () => {
    return (
        <footer className="admin-footer">
            <div className="admin-footer__content">
                <p>&copy; {new Date().getFullYear()} Công ty Vàng Bạc Đá Quý. All rights reserved.</p>
            </div>
        </footer>
    );
};

export default AdminFooter;
