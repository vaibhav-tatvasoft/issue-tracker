import * as Yup from 'yup';

const FormValidation = Yup.object().shape({
  title: Yup.string()
    .min(2, "Title must be at least 2 characters")
    .max(24, "Title cannot exceed 24 characters")
    .required("Please enter a title"),
  id: Yup.number()
    .required("Please enter a value"),
  userId: Yup.number()
    .required("Please enter a value"),
  body: Yup.string()
    .required("Please enter a description")
});

export default FormValidation;
