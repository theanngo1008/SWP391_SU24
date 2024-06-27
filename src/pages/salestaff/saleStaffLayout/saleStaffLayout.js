import React, { memo } from 'react';
import Footer from '../footer/footer';
import Header from '../header/header';

const saleStaffLayout = ({ children, ...props }) => {
    return (
        <div {...props}>
            <Header />
            <main>{children}</main>
            <Footer />
        </div>
    );
};

export default memo(saleStaffLayout);