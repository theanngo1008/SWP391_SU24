import React, { useState, useEffect } from 'react';
import './ManageAccount.scss';

const roles = [
    { value: 'AD', label: 'Admin' },
    { value: 'MN', label: 'Manager' },
    { value: 'US', label: 'User' },
    { value: 'SS', label: 'Staff' },
    { value: 'DS', label: 'Designer' },
    { value: 'PS', label: 'Salesperson' }
];

const statuses = [
    { value: 1, label: 'Active' },
    { value: 2, label: 'Inactive (long time no login)' },
    { value: 3, label: 'Banned' }
];

const ManageAccount = () => {
    const [accounts, setAccounts] = useState([]);
    const [editAccountId, setEditAccountId] = useState(null);
    const [editedAccount, setEditedAccount] = useState({});

    useEffect(() => {
        fetch('http://localhost:5229/api/Account/GetAccounts')
            .then(response => response.json())
            .then(data => setAccounts(data))
            .catch(error => console.error('Error fetching accounts:', error));
    }, []);

    const handleEditClick = (account) => {
        setEditAccountId(account.accId);
        setEditedAccount(account);
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setEditedAccount({ ...editedAccount, [name]: value });
    };

    const handleSaveClick = () => {
        // Thực hiện lưu thay đổi lên backend qua API (sử dụng fetch hoặc axios)
        console.log('Saved account:', editedAccount);
        // Sau khi lưu thành công, cập nhật state
        setAccounts(accounts.map(acc => (acc.accId === editedAccount.accId ? editedAccount : acc)));
        setEditAccountId(null);
    };

    const handleCancelClick = () => {
        setEditAccountId(null);
    };

    return (
        <div className="manage-account">
            <h1>Manage Accounts</h1>
            <table className="account-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Role</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {accounts.map(account => (
                        <tr key={account.accId}>
                            <td>{account.accId}</td>
                            <td>
                                {editAccountId === account.accId ? (
                                    <input
                                        type="text"
                                        name="accName"
                                        value={editedAccount.accName}
                                        onChange={handleChange}
                                    />
                                ) : (
                                    account.accName
                                )}
                            </td>
                            <td>
                                {editAccountId === account.accId ? (
                                    <input
                                        type="email"
                                        name="email"
                                        value={editedAccount.email}
                                        onChange={handleChange}
                                    />
                                ) : (
                                    account.email
                                )}
                            </td>
                            <td>
                                {editAccountId === account.accId ? (
                                    <select
                                        name="role"
                                        value={editedAccount.role}
                                        onChange={handleChange}
                                    >
                                        {roles.map(role => (
                                            <option key={role.value} value={role.value}>
                                                {role.label}
                                            </option>
                                        ))}
                                    </select>
                                ) : (
                                    roles.find(role => role.value === account.role)?.label
                                )}
                            </td>
                            <td>
                                {editAccountId === account.accId ? (
                                    <select
                                        name="status"
                                        value={editedAccount.status}
                                        onChange={handleChange}
                                    >
                                        {statuses.map(status => (
                                            <option key={status.value} value={status.value}>
                                                {status.label}
                                            </option>
                                        ))}
                                    </select>
                                ) : (
                                    statuses.find(status => status.value === account.status)?.label
                                )}
                            </td>
                            <td>
                                {editAccountId === account.accId ? (
                                    <>
                                        <button onClick={handleSaveClick}>Save</button>
                                        <button onClick={handleCancelClick}>Cancel</button>
                                    </>
                                ) : (
                                    <button onClick={() => handleEditClick(account)}>Edit</button>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ManageAccount;
