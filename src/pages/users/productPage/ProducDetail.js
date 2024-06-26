import React, { useState, useEffect } from 'react';
import { Link, useParams } from 'react-router-dom';
import './style.scss';
import AddToCart from 'components/AddToCart';
import axios from 'axios';
import { BiLogoMessenger } from 'react-icons/bi';
import { AiOutlineMessage } from 'react-icons/ai';

const ProductPage = () => {
    const { id } = useParams();
    const [product, setProduct] = useState(null);
    const [quantity, setQuantity] = useState(1);
    const [cartItems, setCartItems] = useState(
        JSON.parse(localStorage.getItem("cartItems")) || []
    );

    useEffect(() => {
        axios.get(`http://localhost:5229/api/Jewelry/${id}`)
            .then(response => {
                setProduct(response.data);
            })
            .catch(error => {
                console.error("There was an error fetching the product!", error);
            });
    }, [id]);

    if (!product) {
        return <div>Product not found</div>;
    }

    const handleQuantityChange = (e) => {
        const value = Math.max(1, Number(e.target.value));
        setQuantity(value);
    };

    const incrementQuantity = () => {
        setQuantity((prevQuantity) => prevQuantity + 1);
    };

    const decrementQuantity = () => {
        setQuantity((prevQuantity) => Math.max(1, prevQuantity - 1));
    };

    const handleAddToCart = () => {
        setCartItems((prevItems) => {
            const existingItemIndex = prevItems.findIndex(
                (item) => item.product.id === product.id
            );

            if (existingItemIndex > -1) {
                // If it exists, update the quantity
                const updatedItems = [...prevItems];
                updatedItems[existingItemIndex].quantity += quantity;
                return updatedItems;
            } else {
                // If it doesn't exist, add it as a new item
                return [...prevItems, { product, quantity }];
            }
        });

        localStorage.setItem("cartItems", JSON.stringify(cartItems));
    };

    return (
        <div className="product-page">
            <div className="product-details">
                <img src={product.image} alt={product.jewelryName} />
                <div className="details">
                    <h1>{product.jewelryName}</h1>
                    <p>{product.description || "No description available"}</p>
                    <p>{product.price || "Price not available"}</p>

                    <div className="actions">
                        <div className="quantity-controls">
                            <button onClick={decrementQuantity}>-</button>
                            <input
                                type="number"
                                value={quantity}
                                onChange={handleQuantityChange}
                            />
                            <button onClick={incrementQuantity}>+</button>
                        </div>
                        <button onClick={handleAddToCart}>Thêm vào giỏ hàng</button>
                        <button>Mua ngay</button>
                        <button>Chat Zalo</button>
                        <a href="/messenger" className="messenger-button">
                            <BiLogoMessenger className="messenger-icon" />
                            Contact Messenger
                        </a>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ProductPage;
