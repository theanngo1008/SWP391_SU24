// src/components/HeaderSalestaff.js
import React from 'react';
import { Link } from 'react-router-dom';
import '../header/header.scss';

const Header = () => {
    return (
        <header className="header-salestaff">
            <nav>
                <ul>
                    <li><Link to="/sale-staff/chat">Chat</Link></li>
                    <li><Link to="/">Logout</Link></li>
                </ul>
            </nav>
        </header>
    );
};

export default Header;
