import React from 'react';
import PropTypes from 'prop-types';
import { withTranslation } from 'react-i18next';
import { ListItem, ListItemIcon, ListItemText, Badge } from '@material-ui/core';

const SidebarItem = (props) => {
    const {
        onClick,
        destination,
        text,
        icon,
        badge,
        badgeValue,
        t } = props;

    const handleDestination = () => {
        onClick(destination);
    };

    return (
        <React.Fragment>
            <ListItem button onClick={destination ? handleDestination : onClick} key={text}>
                <ListItemIcon>
                    {badge
                        ? <Badge badgeContent={badgeValue} color='primary'> {icon} </Badge>
                        : <React.Fragment>{icon}</React.Fragment>
                    }
                </ListItemIcon>
                <ListItemText primary={t(`sidebar.${text}`)} />
            </ListItem>
        </React.Fragment>
    );
}

SidebarItem.propTypes = {
    onClick: PropTypes.func.isRequired,
    destination: PropTypes.string,
    text: PropTypes.string,
    icon: PropTypes.node,
    badge: PropTypes.bool,
    badgeValue: PropTypes.number
};

export default withTranslation()(SidebarItem);