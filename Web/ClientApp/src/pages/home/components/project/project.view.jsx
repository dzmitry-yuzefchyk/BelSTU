import React from 'react';
import { inject, observer } from 'mobx-react';

@inject('rootStore')
@observer
class ProjectView extends React.Component {

    
    render() {
        return("hello");
    }
}

export default ProjectView;