import React, { memo, useEffect, useState } from "react";
import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";
import axios from "axios";
import { AiOutlineMenu, AiOutlinePhone } from "react-icons/ai";
import { Link, useNavigate } from "react-router-dom";
import "./style.scss";

const HomePage = () => {
  const navigate = useNavigate();
  const [isShowCategories, setShowCategories] = useState(true);
  const [products, setProducts] = useState([]);

  const handleImageClick = (id) => {
    navigate(`/product/${id}`);
  };

  const responsive = {
    superLargeDesktop: {
      breakpoint: { max: 4000, min: 3000 },
      items: 5,
    },
    desktop: {
      breakpoint: { max: 3000, min: 1024 },
      items: 4,
    },
    tablet: {
      breakpoint: { max: 1024, min: 464 },
      items: 2,
    },
    mobile: {
      breakpoint: { max: 464, min: 0 },
      items: 1,
    }
  };

  useEffect(() => {
    axios.get('http://localhost:5229/api/Jewelry')
      .then(response => {
        setProducts(response.data);
      })
      .catch(error => {
        console.error("There was an error fetching the products!", error);
      });
  }, []);

  return (
    <div className="container">
      <div className="row hero_categories_container">
        <div className="col-lg-3 hero_categories">
          <div className="hero_categories_all" onClick={() => setShowCategories(!isShowCategories)}>
            <AiOutlineMenu />
            List Jewelry
          </div>
          <ul className={isShowCategories ? "" : "hidden"}>
            <li><Link to={"#"}>Ring</Link></li>
            <li><Link to={"#"}>BraceLet</Link></li>
            <li><Link to={"#"}>NeckLace</Link></li>
          </ul>
        </div>
        <div className="col-lg-9 hero_search_container">
          <div className="hero_search">
            <div className="hero_search_form">
              <form>
                <input type="text" name="search" placeholder="What are you looking for?" />
                <button type="submit">Search</button>
              </form>
            </div>
            <div className="hero_search_phone">
              <div className="hero_search_phone_icon"><AiOutlinePhone /></div>
              <div className="hero_search_phone_text">
                <p>0363433416</p>
                <span>support 24/7</span>
              </div>
            </div>
          </div>
          <div className="hero_item">
            <div className="hero_text">
              <span>LiLiLa Shop</span>
              <h2>Helps you <br /> become noble</h2>
              <p>Fast Delivery</p>
              <Link to="" className="primary-btn">Buy Now</Link>
            </div>
          </div>
        </div>
      </div>
      <div className="container container_categories_slider">
        <hr className="divider" />
        <div className="container_name">Products</div>
        <Carousel responsive={responsive} className="categories_slider">
          {products.map((item, key) => (
            <div
              className="categories_slider_item"
              style={{ backgroundImage: `url(${item.image})` }}
              key={key}
              onClick={() => handleImageClick(item.jewelryId)}
            >
            </div>
          ))}
        </Carousel>
      </div>
    </div>
  );
};

export default memo(HomePage);
