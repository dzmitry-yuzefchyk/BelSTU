import React from 'react';
import { Drawer, withStyles, Divider, List, IconButton } from '@material-ui/core';
import {
    DeveloperBoard as DeveloperBoardIcon,
    AccountBox as AccountIcon,
    Notifications as NotificationsIcon,
    ChevronRight as ChevronRightIcon,
    ChevronLeft as ChevronLeftIcon,
    ExitToApp as SignOutIcon
} from '@material-ui/icons';
import { inject, observer } from 'mobx-react';
import clsx from 'clsx';
import { withRouter } from 'react-router-dom';
import SidebarItem from './sidebar.item';
import { PROJECTS, ACCOUNT, NOTIFICATIONS } from './../../utils/routes';

const drawerWidth = 200;

const styles = theme => ({
    drawer: {
        width: drawerWidth,
        flexShrink: 0,
        whiteSpace: 'nowrap',
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
@observer
@withStyles(styles)
@withRouter
class Sidebar extends React.Component {

    constructor() {
        super();

        this.state = {
            isOpen: true
        };

        this.toggleSidebar = this.toggleSidebar.bind(this);
        this.handleGoto = this.handleGoto.bind(this);
    }

    toggleSidebar() {
        this.setState(prevState => ({
            isOpen: !prevState.isOpen
        }));
    }

    handleGoto(destination) {
        const { history } = this.props;
        history.push(destination)
    }

    render() {
        const { classes, rootStore } = this.props;
        const { isOpen } = this.state;        
        const { userStore } = rootStore;

        return (
            <Drawer
                variant='permanent'
                className={clsx(classes.drawer, {
                    [classes.drawerOpen]: isOpen,
                    [classes.drawerClose]: !isOpen,
                })}
                classes={{
                    paper: clsx({
                        [classes.drawerOpen]: isOpen,
                        [classes.drawerClose]: !isOpen,
                    }),
                }}
                open={isOpen}
            >
                <div className={classes.toolbar}>
                    <IconButton onClick={this.toggleSidebar}>
                        {isOpen ? <ChevronRightIcon /> : <ChevronLeftIcon />}
                    </IconButton>
                </div>
                <Divider />
                <List>
                    <SidebarItem
                        onClick={this.handleGoto}
                        destination={PROJECTS}
                        text={'Projects'}
                        icon={<DeveloperBoardIcon />}
                    />
                </List>
                <Divider />
                <List>
                    <SidebarItem
                        onClick={this.handleGoto}
                        destination={ACCOUNT}
                        text={'Account'}
                        icon={<AccountIcon />}
                    />
                    <SidebarItem
                        onClick={this.handleGoto}
                        destination={NOTIFICATIONS}
                        text={'Notifications'}
                        icon={<NotificationsIcon />}
                        badge
                        badgeValue={3}
                    />
                </List>
                <Divider />
                <List>
                    <SidebarItem
                        onClick={userStore.signOut}
                        text={'Sign-Out'}
                        icon={<SignOutIcon />}
                    />
                </List>
            </Drawer>
        );
    }
}

export default Sidebar;