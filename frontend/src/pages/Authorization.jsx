import {useState} from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/Authorization.css';

function Authorization() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    // тут хранятся данные для входа
    const validCredentials = [
        {username: 'sosal', password: 'sosal'},
        {username: 'pedic', password: 'rashitov'}
    ]

    // обработка события
    const handleSubmit = async (e) => {
        e.preventDefault();

        const matchedUser = validCredentials.find(
            cred => cred.username === username && cred.password === password
        );

        if (matchedUser) {
            navigate('/courses');
        } else {
            console.log('No')
        }
    }

    return (
        <div className="login-form">
            <h2>Вход</h2>
            <form onSubmit={handleSubmit}>
                <div className='form-group'>
                    <label htmlFor='username'>Username:</label>
                    <input
                        type='text'
                        id='username'
                        name='username'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                        autoComplete="username"/>
                </div>
                <div className='form-group'>
                    <label htmlFor='password'>Password:</label>
                    <input
                        type='password'
                        id='password'
                        name='password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        autoComplete="current-password"/>
                </div>
                <button type="submit" className="login-button">Войти</button>
            </form>
        </div>
    );
}


export default Authorization;