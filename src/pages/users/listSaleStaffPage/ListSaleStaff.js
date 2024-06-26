import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Button } from '@mui/material';
import styles from './ListSaleStaff.scss';

const ListSaleStaff = () => {
    const [saleStaff, setSaleStaff] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5229/api/Account/GetAccounts')
            .then(response => {
                // Lọc các tài khoản có role là 'SS'
                const ssAccounts = response.data.filter(account => account.role === 'SS');
                setSaleStaff(ssAccounts);
            })
            .catch(error => {
                console.error('There was an error fetching the accounts!', error);
            });
    }, []);

    return (
        <div className={styles.listSaleStaffContainer}>
            <h1 className={styles.listSaleStaffTitle}>Sale Staff List</h1>
            <ul className={styles.listSaleStaffList}>
                {saleStaff.map(staff => (
                    <li key={staff.accId} className={styles.listSaleStaffItem}>
                        <span className={styles.listSaleStaffItemText}>{staff.accName}</span>
                        <Button className={styles.listSaleStaffChatButton} variant="contained">Chat</Button>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default ListSaleStaff;
