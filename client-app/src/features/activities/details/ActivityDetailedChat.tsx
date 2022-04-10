import { Formik, Form, Field, FieldProps } from 'formik';
import { observer } from 'mobx-react-lite'
import React, { useEffect } from 'react'
import { Link } from 'react-router-dom';
import {Segment, Header, Comment, Loader} from 'semantic-ui-react'
import { useStore } from '../../../app/stores/store';
import * as Yup from 'yup';
import { formatDistanceToNow } from 'date-fns';

interface Props {
    activityId: string;
}

export default observer(function ActivityDetailedChat({activityId}: Props) {
    const {commentStore} = useStore();

    useEffect(() => {
        // check if we have an activityId - it`s important 
        // because according to this activityId SignalR groups will be formed
        if (activityId) {
            commentStore.createHubConnection(activityId);
        }
        // when the user follow another activity, the comments of the current activity are disposed for him
        return () => {
            commentStore.clearComments();
        }
    }, [commentStore, activityId])

    return (
        <>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{border: 'none'}}>
                <Header>Chat about this event</Header>
            </Segment>
            <Segment attached clearing>
                <Formik
                    onSubmit={(values, {resetForm}) => commentStore.addComment(values).then(() => resetForm())}
                    initialValues={{body: ''}}
                    validationSchema={Yup.object({
                        body: Yup.string().required()
                    })}>
                    {({isSubmitting, isValid, handleSubmit}) => (                    
                        <Form className="ui form">
                            <Field name='body'>
                                {(props: FieldProps) => (
                                    <div style={{position: 'relative'}}>
                                        <Loader active={isSubmitting} />
                                        <textarea placeholder='Enter your comments (Press enter to submit)' 
                                            rows={2}
                                            {...props.field} 
                                            onKeyPress={e => {
                                                // if we press 'Enter', the form is submitted;
                                                // if we press 'Shift + Enter', we jump to a new line inside a textarea
                                                if (e.key === 'Enter' && e.shiftKey) return;
                                                if (e.key === 'Enter' && !e.shiftKey) {
                                                    e.preventDefault();
                                                    isValid && handleSubmit();
                                                }
                                            }}/>
                                    </div>
                                )}
                            </Field>
                        </Form>
                    )}
                </Formik>
                <Comment.Group>
                    {commentStore.comments.map(comment => (
                        <Comment key={comment.id}> 
                            <Comment.Avatar src={comment.image || '/assets/user.png'} />
                            <Comment.Content>
                                <Comment.Author as={Link} to={`/profiles/${comment.username}`}>
                                    {comment.displayName}
                                </Comment.Author>
                                <Comment.Metadata>
                                    <div>{formatDistanceToNow(comment.createdAt)} ago</div>
                                </Comment.Metadata>
                                <Comment.Text style={{whiteSpace: 'pre-wrap'}}>{comment.body}</Comment.Text>
                            </Comment.Content>
                        </Comment>
                    ))}
                </Comment.Group>
            </Segment>
        </>
    )
})
