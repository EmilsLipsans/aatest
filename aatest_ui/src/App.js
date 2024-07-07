import logo from './car-logo.svg';
import './App.css';
import { Cars } from './Cars';
import { CarBrands } from './CarBrands';
import { BrowserRouter, Routes, Route, NavLink } from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
      <div className="App container">
        <img src={logo} alt="Logo" style={{ height: '80px' }} />
        <h3 className="d-flex justify-content-center m-3">
          Car manager
        </h3>
        <nav className="navbar navbar-expand-sm d-flex justify-content-center">
          <ul className="navbar-nav">
            <li className="nav-item m-1">
              <NavLink className="btn btn-light btn-outline-primary" to="/cars">
                Cars
              </NavLink>
            </li>
            <li className="nav-item m-1">
              <NavLink className="btn btn-light btn-outline-primary" to="/carbrands">
                Car brands
              </NavLink>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path='/cars' element={<Cars />} />
          <Route path='/carbrands' element={<CarBrands />} />
        </Routes>

        <footer className="footer mt-auto py-3 bg-light">
          <div className="container d-flex justify-content-center">
            <span className="text-muted">Version 1.0.0</span>
          </div>
        </footer>
      </div>

    </BrowserRouter>
  );
}

export default App;