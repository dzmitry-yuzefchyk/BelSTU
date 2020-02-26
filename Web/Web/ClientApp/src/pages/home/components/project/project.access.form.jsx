import React, { useState } from 'react';
import * as R from 'ramda';
import { inject, observer } from 'mobx-react';
import {
    withStyles,
    Box,
    Paper,
    Button,
    TextField,
    Checkbox,
    Select,
    MenuItem,
    FormControl,
    InputLabel,
    FormControlLabel,
    IconButton,
    TableHead,
    TableCell,
    TableRow,
    Table,
    TableBody,
    TablePagination
} from '@material-ui/core';
import { RemoveCircle as RemoveCircleIcon } from '@material-ui/icons'
import { withTranslation } from 'react-i18next';
import AddUserModal from './add.user.modal';

const userActions = [
    { title: 'CREATE_BOARD', value: 0 },
    { title: 'UPDATE_BOARD', value: 1 },
    { title: 'DELETE_BOARD', value: 2 },

    { title: 'CREATE_TASK', value: 3 },
    { title: 'UPDATE_TASK', value: 4 },
    { title: 'DELETE_TASK', value: 5 },

    { title: 'CREATE_COMMENT', value: 6 },

    { title: 'UPDATE_PROJECT', value: 7 },
    { title: 'CHANGE_SECURITY', value: 8 },
    { title: 'DELETE_PROJECT', value: 9 }
];

const styles = theme => ({
    form: {
        width: '100%'
    },
    paper: {
        overflowX: 'auto'
    },
    formInputBox: {
        display: 'flex',
        justifyContent: 'center',
        paddingTop: '0.225rem',
        paddingBottom: '0.225rem'
    },
    button: {
        height: '2.125rem',
        margin: '0.425rem'
    }
});

const ProjectAccessForm = props => {
    const { t, classes, projectId, fetchAccess } = props;
    const { projectStore, modalStore } = props.rootStore;
    const { access } = projectStore;
    const [ page, setPage ] = useState(0);
    const updateAccess = async () => {
        await projectStore.updateAccess(Number(projectId));
    };

    const handleChangePage = (event, page) => {
        setPage(page);
        fetchAccess(page);
    };

    const handleAddUser = () => {
        modalStore.show(<AddUserModal projectId={projectId} callback={fetchAccess} />);
    };

    const handleCheckboxChange = (email, action) => event => {
        access.users = access.users.map(access => {
            if (access.email === email) {
                access.actions = access.actions.map(a => {
                    if (a.action === action) {
                        a.allowed = event.target.checked;
                    }

                    return a;
                });
            }

            return access;
        });
    };

    const handleRemoveUser = email => async event => {
        const projectUser = {
            projectId: Number(projectId),
            email
        };
        await projectStore.removeFromProject(projectUser);
    };

    return (
        <Box display='flex' flexDirection='row'>
            <Box className={classes.form}>
                <Paper elevation={2} className={classes.paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>
                                    {t('access.email')}
                                </TableCell>
                                {userActions.map(action =>
                                    <TableCell key={action.title}>
                                        {t(`access.${action.title}`)}
                                    </TableCell>
                                )}
                                <TableCell>
                                    {t('access.blocked')}
                                </TableCell>
                                <TableCell>
                                    {t('access.remove')}
                                </TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {access.users.map(user =>
                                <TableRow key={user.email}>
                                    <TableCell>
                                        {user.email}
                                    </TableCell>
                                    {user.actions.map(action =>
                                        <TableCell key={`${user.email}-${action.action}`}>
                                            <Checkbox
                                                disabled={user.isAdmin || user.changingBlocked}
                                                checked={action.allowed}
                                                onChange={handleCheckboxChange(user.email, action.action)}
                                            />
                                        </TableCell>
                                    )}
                                    <TableCell>
                                        <Checkbox
                                            disabled={true}
                                            checked={user.changingBlocked}
                                        />
                                    </TableCell>
                                    <TableCell>
                                        <IconButton disabled={user.isAdmin} onClick={handleRemoveUser(user.email)} >
                                            <RemoveCircleIcon />
                                        </IconButton>
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                    <TablePagination
                        component='div'
                        count={access.total || 0}
                        rowsPerPage={20}
                        page={page}
                        onChangePage={handleChangePage}
                        rowsPerPageOptions={[20]}
                    />

                    <Box className={classes.formInputBox} display='flex' justifyContent='space-around' flexDirection='row'>
                        <Button className={classes.button} variant='contained' color='primary' onClick={updateAccess}>
                            {t('forms.update')}
                        </Button>

                        <Button className={classes.button} variant='contained' color='primary' onClick={handleAddUser}>
                            {t('forms.add user')}
                        </Button>
                    </Box>
                </Paper>
            </Box>
        </Box>
    );
}

export default R.compose(
    inject('rootStore'),
    withTranslation(),
    withStyles(styles),
    observer,
)(ProjectAccessForm);
