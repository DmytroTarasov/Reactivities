import { observer } from 'mobx-react-lite';
import React from 'react';
import MyTextArea from '../../app/common/form/MyTextArea';
import MyTextInput from '../../app/common/form/MyTextInput';
import { Formik, Form } from "formik";
import * as Yup from 'yup';
import { useStore } from '../../app/stores/store';
import { Button } from 'semantic-ui-react';
import { Profile } from '../../app/models/profile';

interface Props {
    setEditMode: (editMode: boolean) => void
}

export default observer(function ProfileEditForm({setEditMode} : Props) {
    const {profileStore: {profile, updateProfile}} = useStore();

    const validationSchema = Yup.object({
        displayName: Yup.string().required('Display name is required'),
    })

    function handleFormSubmit(profile: Partial<Profile>) {
        updateProfile(profile).then(() => setEditMode(false));
    }

    return (
        <Formik
            validationSchema={validationSchema}
            enableReinitialize 
            initialValues={profile!} 
            onSubmit={values => handleFormSubmit(values)}>
                {({handleSubmit, isValid, isSubmitting, dirty}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='displayName' placeholder='Display Name' />
                        <MyTextArea name='bio' placeholder='Add your bio' rows={5} />
                        <Button 
                            disabled={isSubmitting || !dirty || !isValid}
                            loading={isSubmitting} 
                            floated='right' 
                            positive 
                            type='submit' 
                            content='Update profile' />
                    </Form>
                )}
        </Formik>
    )
})