import React, { useEffect, useState } from 'react';
import { db } from 'firebaseConfig';
import { collection, addDoc, query, orderBy, onSnapshot, where } from 'firebase/firestore';
import { Button, TextField, List, ListItem, ListItemText, Typography, Box } from '@mui/material';
import axios from 'axios';
import './Chat.scss'; // Import file SCSS

const Chat = () => {
    const [message, setMessage] = useState('');
    const [messages, setMessages] = useState([]);
    const [ssUsers, setSsUsers] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null); // State to track selected user

    // Đọc thông tin người dùng từ localStorage
    const currentUser = JSON.parse(localStorage.getItem('account'));

    useEffect(() => {
        const fetchSsUsers = async () => {
            try {
                const response = await axios.get('http://localhost:5229/api/Account/GetAccounts');
                const ssUsers = response.data.filter(user => user.role === 'SS');
                setSsUsers(ssUsers);
                console.log('Fetched SS Users:', ssUsers); // Logging for debugging
            } catch (error) {
                console.error('Error fetching SS users:', error);
            }
        };

        fetchSsUsers();
    }, []);

    useEffect(() => {
        if (currentUser) {
            const q = query(
                collection(db, 'messages'),
                where('users', 'array-contains', currentUser.accId),
                orderBy('timestamp')
            );

            const unsubscribe = onSnapshot(q, (snapshot) => {
                const fetchedMessages = snapshot.docs.map(doc => doc.data());
                setMessages(fetchedMessages);
                console.log('Fetched Messages:', fetchedMessages); // Logging for debugging
            });

            return () => unsubscribe();
        }
    }, [currentUser]);

    const handleSendMessage = async () => {
        if (message.trim() && currentUser && currentUser.accId && selectedUser && selectedUser.accId) {
            await addDoc(collection(db, 'messages'), {
                text: message,
                senderId: currentUser.accId,
                senderName: currentUser.accName, // Lưu thêm tên người gửi
                receiverId: selectedUser.accId,
                users: [currentUser.accId, selectedUser.accId],
                timestamp: new Date()
            });
            setMessage('');
        }
    };

    if (!selectedUser) {
        return (
            <div className="chatContainer">
                <Typography variant="h6">SS Users</Typography>
                <List>
                    {ssUsers.map((user, index) => (
                        <ListItem button key={index} onClick={() => setSelectedUser(user)}>
                            <ListItemText primary={user.accName} />
                        </ListItem>
                    ))}
                </List>
                <Typography variant="h6" style={{ marginTop: '20px' }}>All Messages</Typography>
                <List>
                    {messages.map((msg, index) => (
                        <ListItem key={index}>
                            <ListItemText primary={`${msg.senderName}: ${msg.text}`} secondary={`To: ${msg.receiverId}`} /> {/* Hiển thị tên người gửi và người nhận */}
                        </ListItem>
                    ))}
                </List>
            </div>
        );
    }

    return (
        <div className="chatContainer">
            <Typography variant="h6">Chat with {selectedUser.accName}</Typography>
            <Button onClick={() => setSelectedUser(null)}>Back to User List</Button>
            <List className="messageList">
                {messages
                    .filter(msg => (msg.senderId === currentUser.accId && msg.receiverId === selectedUser.accId) ||
                        (msg.senderId === selectedUser.accId && msg.receiverId === currentUser.accId))
                    .map((msg, index) => (
                        <ListItem key={index} className={msg.senderId === currentUser.accId ? "sentMessage" : "receivedMessage"}>
                            <ListItemText primary={`${msg.senderName}: ${msg.text}`} /> {/* Hiển thị tên người gửi */}
                        </ListItem>
                    ))}
            </List>
            <Box display="flex" alignItems="center">
                <TextField
                    label="Message"
                    variant="outlined"
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    fullWidth
                />
                <Button variant="contained" color="primary" onClick={handleSendMessage}>
                    Send
                </Button>
            </Box>
        </div>
    );
};

export default Chat;
