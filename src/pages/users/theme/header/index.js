import { memo, useState, useEffect } from "react";
import "./style.scss";
import { AiOutlineFacebook, AiOutlineInstagram, AiOutlineLinkedin, AiOutlineMail, AiOutlineMenu, AiOutlinePhone, AiOutlineShoppingCart, AiOutlineUser } from "react-icons/ai";
import { BiLogoMessenger, BiLogoYoutube, BiPhone, BiUser } from "react-icons/bi";
import { Link, Navigate } from "react-router-dom";
import logo from './logoHeader.jpg';
import logo1 from './logo1.webp'
import { ROUTERS } from "utils/router";
import Login from 'components/Login';
import Register from 'components/Register';
import { useNavigate } from 'react-router-dom';
const Header = () => {
  const [showPopup, setShowPopup] = useState(null); // null: không popup, 'login': login, 'register': register
  const [user, setUser] = useState(null);
  const [accName, setAccName] = useState('');
  const navigate = useNavigate();
  useEffect(() => {
    const savedUser = localStorage.getItem('user');
    const savedAccName = localStorage.getItem('accName');

    if (savedUser && savedAccName) {
      setUser(savedUser);
      setAccName(savedAccName);
    }
  }, []);
  const handleLoginClick = () => {
    setShowPopup('login');
  };

  const handleRegisterClick = () => {
    setShowPopup('register');
  };

  const handleClosePopup = () => {
    setShowPopup(null); // Đóng tất cả popup
  };
  const handleLogout = () => {
    setUser(null);
    setAccName('');
    localStorage.removeItem('user');
    localStorage.removeItem('accName');
    navigate('/');
  };
  const [isShowHumberger, setShowHumberger] = useState(false);
  const [menus] = useState([
    {
      name: "HOME",
      path: ROUTERS.USER.HOME,
    },
    {
      name: "JEWELRY",
      path: ROUTERS.USER.JEWELRY,
    },
    {
      name: "MENS",
      path: ROUTERS.USER.MENS,
      isShowSubmenu: false,
      child: [
        {
          name: "Ring",
          path: ROUTERS.USER.RICHMEN,
        },
        {
          name: "BraceLet",
          path: ROUTERS.USER.BRACELETMENPAGE,
        },
        {
          name: "NeckLace",
          path: ROUTERS.USER.NECKLACEMEN,
        },
      ],
    },
    {
      name: "WOMENS",
      path: ROUTERS.USER.WOMENS,
      isShowSubmenu: false,
      child: [
        {
          name: "Ring",
          path: "",
        },
        {
          name: "BraceLet",
          path: "",
        },
        {
          name: "NeckLace",
          path: "",
        },
      ],
    },
    {
      name: "JEWELRY PROCESSING",
      path: ROUTERS.USER.PROCESSING,
    },

  ])
  // Hàm định dạng số thành giá tiền USD
  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  };

  return (
    <>
      <div className={`humberger_menu_overlay${isShowHumberger ? " active" : ""}`}
        onClick={() => setShowHumberger(false)}
      />
      <div
        className={`humberger_menu_wrapper${isShowHumberger ? " show" : ""}`}
      >
        <div className="header_logo">
          <h1>LILILA Shop</h1>
        </div>
        <div className="humberger_menu_cart">
          <ul>
            <li>
              <Link to={""}>
                <AiOutlineShoppingCart /><span>1</span>
              </Link>
            </li>
          </ul>
          <div className="header_cart_price">
            Cart:<span>{formatPrice(100)}</span>
          </div>
        </div>
        <div className="humberger_menu_widget">
          <div className="header_top_right_auth">
            <Link to={""}>
              <BiUser /> Login
            </Link>
          </div>
        </div>
        <div className="humberger_menu_nav1">
          <ul>
            <li>
              <Link to={""}>Account Information </Link>
            </li>
          </ul>
        </div>
        <div className="humberger_menu_nav">
          <ul>
            <li>
              <Link to={""}>Contact Us </Link>
            </li>
          </ul>
        </div>
        <div className="header_top_right_social">
          <Link to={"https://www.facebook.com/profile.php?id=61560052164888"}>
            <AiOutlineFacebook />
          </Link>
          <Link to={""}>
            <AiOutlineInstagram />
          </Link>
          <Link to={""}>
            <AiOutlineLinkedin />
          </Link>
          <Link to={""}>
            <BiLogoYoutube />
          </Link>
          <Link to={"https://www.facebook.com/messages/t/325097017352199"} className="icon">
            <BiLogoMessenger />
          </Link>

        </div>
        <div className="humberger_menu_contact">
          <ul>
            <li>
              <i className="fa fa-envelope">jewelryshop@gmail.com</i>
            </li>
            <li>free shipping from {formatPrice(200)}</li>
          </ul>
        </div>
      </div>
      <div className="header_top">
        <div className="container">
          <div className="row">
            <div className="col-6 header_top_left"  >
              <img src={logo} alt="Logo" className="logo" />
              <ul>
                <li className="logo11">
                  <img src={logo1} alt="Logo1" className="logo1" />
                  Showroom System
                </li>
                <li>
                  <AiOutlineMail />
                  Jewelryproduction@gmail.com
                </li>
                <li>
                  Address:20/2 Street 904, District 9, Ho Chi Minh City
                </li>
              </ul>
            </div>
            <div className="col-6 header_top_right" >
              <ul>
                <li>
                  <Link to={"https://www.facebook.com/profile.php?id=61560052164888"} className="icon">
                    <AiOutlineFacebook />
                  </Link>
                </li>
                <li>
                  <Link to={"https://www.facebook.com/messages/t/325097017352199"} className="icon">
                    <BiLogoMessenger />
                  </Link>
                </li>
                <li>
                  <Link to={""} className="icon">
                    <BiPhone />
                  </Link>
                </li>
                <li>
                  <Link to={"/cart"} className="icon">
                    <AiOutlineShoppingCart />
                  </Link>
                </li>
                <li>
                  <Link to={"/profile"} className="icon">
                    <AiOutlineUser />
                  </Link>
                  <div>
                    {!user ? (
                      <>
                        <button onClick={handleLoginClick}>Đăng nhập</button>
                        <label> / </label>
                        <button onClick={handleRegisterClick}>Đăng ký</button>
                      </>
                    ) : (
                      <>
                        <span>{accName}</span>
                        <button onClick={handleLogout}>Logout</button>
                      </>
                    )}

                    {showPopup === 'login' && (
                      <Login
                        toggle={handleClosePopup}
                        setUser={(user) => {
                          setUser(user);
                          localStorage.setItem('user', user);
                        }}
                        setAccName={(name) => {
                          setAccName(name);
                          localStorage.setItem('accName', name);
                        }}
                      />
                    )}
                    {showPopup === 'register' && <Register toggle={handleClosePopup} />}
                  </div>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      <div className="color_header">
        <div className="container">
          <div className="row">
            <div className="col-xl-3">
              <div className="header_logo">
                <h1>LILILA SHOP</h1>
              </div>
            </div>
            <div className="col-xl-6">
              <nav className="header_menu">
                <ul>
                  {
                    menus?.map((menu, menuKey) => (
                      <li key={menuKey} className={menuKey === 0 ? "header_menu1" : ""} >
                        <Link to={menu?.path}>{menu?.name}</Link>
                        {
                          menu.child && (
                            <ul className="header_menu_dropdown">
                              {menu.child.map((childItem, childKey) => (
                                <li key={`${menuKey}-${childKey}`}>
                                  <Link to={childItem.path}>{childItem.name}</Link>
                                </li>
                              ))}
                            </ul>
                          )
                        }
                      </li>
                    ))}
                </ul>
              </nav>
            </div>
            <div className="col-lg-3">
              <div className="header_cart">
                <div className="header_cart_price">
                  <span>{formatPrice(1000)}</span>
                </div>
                <ul>
                  <li>
                    <Link to="/cart">
                      <AiOutlineShoppingCart /> <span className="sizecart">5</span>
                    </Link>
                  </li>
                </ul>
                <div className="listmenu">
                  <AiOutlineMenu
                    onClick={() => setShowHumberger(true)}
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

    </>
  );
};

export default memo(Header);

