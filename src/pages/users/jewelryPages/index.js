import React, { memo, useState, useEffect } from "react";
import ReactPaginate from "react-paginate";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import "./style.scss";

const ITEMS_PER_PAGE = 16;

const JewelryPages = () => {
    const [currentPage, setCurrentPage] = useState(0);
    const [products, setProducts] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        axios.get('http://localhost:5229/api/Jewelry')
            .then(response => {
                setProducts(response.data.map(item => ({
                    id: item.jewelryId,
                    src: item.image,
                    name: item.jewelryName || "No Name",
                    price: item.price || "0",
                    sold: item.sold || 0,
                    rating: item.rating || 0,
                    reviews: item.reviews || 0,
                    promotion: item.promotion || "Tặng trang sức bộ sưu tập Family Infinity",
                    code: item.code || "XMXMY008154",
                    fastDelivery: item.fastDelivery || true
                })));
            })
            .catch(error => {
                console.error("There was an error fetching the images!", error);
            });
    }, []);

    const handlePageClick = ({ selected }) => {
        setCurrentPage(selected);
    };

    const handleImageClick = (id) => {
        navigate(`/product/${id}`);
    };

    const offset = currentPage * ITEMS_PER_PAGE;
    const currentProducts = products.slice(offset, offset + ITEMS_PER_PAGE);

    return (
        <div className="container container_categories_slider">
            <div className="container_categories_slider">
                <div className="images-grid">
                    {currentProducts.map((product, index) => (
                        <div
                            key={index}
                            className="product-item"
                            onClick={() => handleImageClick(product.id)}
                        >
                            <div className="image" style={{ backgroundImage: `url(${product.src})` }}></div>
                            <div className="product-details">
                                {product.fastDelivery && <div className="fast-delivery">FAST</div>}
                                <p className="name">{product.name}</p>
                                <p className="code">{product.code}</p>
                                <p className="price">{product.price}₫</p>
                                <p className="promotion">{product.promotion}</p>
                                <div className="rating-sold">
                                    <span className="rating">⭐ {product.rating} ({product.reviews} reviews)</span>
                                    <span className="sold">{product.sold} đã bán</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
                <ReactPaginate
                    previousLabel={"Previous"}
                    nextLabel={"Next"}
                    breakLabel={"..."}
                    pageCount={Math.ceil(products.length / ITEMS_PER_PAGE)}
                    marginPagesDisplayed={2}
                    pageRangeDisplayed={5}
                    onPageChange={handlePageClick}
                    containerClassName={"pagination"}
                    activeClassName={"active"}
                />
            </div>
        </div>
    );
};

export default memo(JewelryPages);
