import './Form.css';
import { useFormik } from 'formik';
import FormValidation from './schemas'; // Your validation schema
import { useEffect, useState } from 'react';
import axios from 'axios';

export default function Form() {
  const [arrObject, setArrObject] = useState([]);
  const [apiData, setApiData] = useState([]);

  const apiClient = axios.create({
    baseURL: 'https://jsonplaceholder.typicode.com',
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await apiClient.get('/posts');
        setApiData(response.data);
        console.log(response.data);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  const formik = useFormik({
    initialValues: {
      userId: '',
      id: '',
      title: '',
      body: '',
    },
    validationSchema: FormValidation,
    onSubmit: (values) => {
      setArrObject((prevArray) => [...prevArray, values]);
    },
  });

  useEffect(() => {
    console.log(arrObject);
  }, [arrObject]);

  return (
    <div className="form-container">
      <h2>Create New Post</h2>
      <form onSubmit={formik.handleSubmit}>
        <div className="form-group">
          <label htmlFor="userId">User ID:</label>
          <input
            type="number"
            id="userId"
            name="userId"
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            value={formik.values.userId}
          />
          {formik.touched.userId && formik.errors.userId ? (
            <div className="error">{formik.errors.userId}</div>
          ) : null}
        </div>

        <div className="form-group">
          <label htmlFor="id">ID:</label>
          <input
            type="number"
            id="id"
            name="id"
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            value={formik.values.id}
          />
          {formik.touched.id && formik.errors.id ? (
            <div className="error">{formik.errors.id}</div>
          ) : null}
        </div>

        <div className="form-group">
          <label htmlFor="title">Title:</label>
          <input
            type="text"
            id="title"
            name="title"
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            value={formik.values.title}
          />
          {formik.touched.title && formik.errors.title ? (
            <div className="error">{formik.errors.title}</div>
          ) : null}
        </div>

        <div className="form-group">
          <label htmlFor="body">Body:</label>
          <textarea
            id="body"
            name="body"
            rows="5"
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            value={formik.values.body}
          ></textarea>
          {formik.touched.body && formik.errors.body ? (
            <div className="error">{formik.errors.body}</div>
          ) : null}
        </div>

        <button type="submit" className="submit-btn">
          Submit
        </button>
      </form>
    </div>
  );
}
