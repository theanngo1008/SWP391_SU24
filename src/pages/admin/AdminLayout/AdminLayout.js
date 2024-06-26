import React, { memo } from 'react';
import AdminFooter from '../footer/AdminFooter';
import AdminHeader from '../header/AdminHeader';

const AdminLayout = ({ children, ...props }) => {
    return (
        <div {...props}>
            <AdminHeader />
            <main>{children}</main>
            <AdminFooter />
        </div>
    );
};

export default memo(AdminLayout);
