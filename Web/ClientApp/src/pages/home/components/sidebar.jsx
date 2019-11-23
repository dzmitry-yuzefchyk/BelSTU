import React from 'react';
import { Drawer, withStyles, Divider, List, IconButton } from '@material-ui/core';
import {
    DeveloperBoard as DeveloperBoardIcon,
    Notifications as NotificationsIcon,
    ChevronRight as ChevronRightIcon,
    ChevronLeft as ChevronLeftIcon,
    ExitToApp as SignOutIcon
} from '@material-ui/icons';
import { inject, observer } from 'mobx-react';
import clsx from 'clsx';
import { withRouter } from 'react-router-dom';
import SidebarItem from './sidebar.item';
import { withTranslation } from 'react-i18next';
import { PROJECTS, ACCOUNT, NOTIFICATIONS } from './../../../utils/routes';

const drawerWidth = 200;

const styles = theme => ({
    icon: {
        color: theme.palette.primary.contrastText
    },
    img: {
        maxWidth: 24,
        maxHeight: 24,
        borderRadius: '100px'
    },
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
        whiteSpace: 'nowrap'
    },
    paper: {
        background: theme.palette.primary.main,
        color: theme.palette.primary.contrastText
    },
    drawerOpen: {
        width: drawerWidth,
        transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.enteringScreen,
        }),
    },
    drawerClose: {
        transition: theme.transitions.create('width', {
            easing: theme.transitions.easing.sharp,
            duration: theme.transitions.duration.leavingScreen,
        }),
        overflowX: 'hidden',
        width: theme.spacing(7) + 6
    },
    toolbar: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'flex-end',
        padding: theme.spacing(0, 1),
        ...theme.mixins.toolbar,
    }
});

@inject('rootStore')
@withStyles(styles)
@withTranslation()
@withRouter
@observer
class Sidebar extends React.Component {

    constructor() {
        super();

        this.toggleSidebar = this.toggleSidebar.bind(this);
        this.handleGoto = this.handleGoto.bind(this);
    }

    toggleSidebar() {
        const { userStore } = this.props.rootStore;
        const isNavOpen = userStore.isNavOpen;
        userStore.isNavOpen = !isNavOpen;
    }

    handleGoto(destination) {
        const { history } = this.props;
        history.push(destination)
    }

    render() {
        const { classes, rootStore, t } = this.props;
        const { userStore, notificationStore } = rootStore;
        const isOpen = userStore.isNavOpen;

        return (
            <Drawer
                variant='permanent'
                className={clsx(classes.drawer, {
                    [classes.drawerOpen]: isOpen,
                    [classes.drawerClose]: !isOpen,
                })}
                classes={{
                    paper: clsx(classes.paper, {
                        [classes.drawerOpen]: isOpen,
                        [classes.drawerClose]: !isOpen,
                    }),
                }}
                open={isOpen}
            >
                <div className={classes.toolbar}>
                    <IconButton onClick={this.toggleSidebar}>
                        {isOpen ? <ChevronLeftIcon className={classes.icon} /> : <ChevronRightIcon className={classes.icon} />}
                    </IconButton>
                </div>
                <Divider />
                <List>
                    <SidebarItem
                        onClick={this.handleGoto}
                        destination={PROJECTS}
                        text={t('sidebar.Projects')}
                        icon={<DeveloperBoardIcon className={classes.icon} />}
                    />
                </List>
                <Divider />
                <List>
                    <SidebarItem
                        onClick={this.handleGoto}
                        destination={ACCOUNT}
                        text={userStore.user.profile ? `@${userStore.user.profile.tag}` : ''}
                        icon={
                            <img
                                className={classes.img}
                                src={userStore.user.profile ? userStore.user.profile.icon : ''}
                            />}
                    />
                    <SidebarItem
                        onClick={this.handleGoto}
                        destination={NOTIFICATIONS}
                        text={t('sidebar.Notifications')}
                        icon={<NotificationsIcon className={classes.icon} />}
                        badge
                        badgeValue={notificationStore.amount}
                    />
                </List>
                <Divider />
                <List>
                    <SidebarItem
                        onClick={userStore.signOut}
                        text={t('sidebar.Sign-Out')}
                        icon={<SignOutIcon className={classes.icon} />}
                    />
                </List>
            </Drawer>
        );
    }
}

export default Sidebar;