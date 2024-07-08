import React, { Component } from 'react';
import { variables } from './Variables.js';
import './CarBrands.css';


export class CarBrands extends Component {

    constructor(props) {
        super(props);

        this.state = {
            brands: [],
            brandName: "",
            isBrandChanged: false,
            currentPage: 1, 
            itemsPerPage: 7,
            BrandIdFilter: "",
            BrandNameFilter: "",
            brandsNoFilter: [],
        }
    }

    FilterBrands(data) {
        const { BrandIdFilter, BrandNameFilter, brandsNoFilter } = this.state;

        let filteredData = [];

        if (data == null) {
            filteredData = brandsNoFilter.filter(b => (
                b.id.toString().toLowerCase().includes(BrandIdFilter.toString().trim().toLowerCase()) &&
                b.brand_name.toString().toLowerCase().includes(BrandNameFilter.toString().trim().toLowerCase())
            ));
        } else {
            filteredData = data.filter(b => (
                b.id.toString().toLowerCase().includes(BrandIdFilter.toString().trim().toLowerCase()) &&
                b.brand_name.toString().toLowerCase().includes(BrandNameFilter.toString().trim().toLowerCase())
            ));
            this.setState({ brandsNoFilter: data });
        }

        this.setState({ brands: filteredData });
    }

    changeBrandIdFilter = (e) => {
        this.state.BrandIdFilter = e.target.value;
        this.FilterBrands(null);
    }
    changeBrandNameFilter = (e) => {
        this.state.BrandNameFilter = e.target.value;
        this.FilterBrands(null);
    }

    refreshList() {
        fetch(variables.API_URL + 'CarBrand/GetCarBrands')
            .then(response => response.json())
            .then(data => {
                if (this.state.BrandIdFilter != "" || this.state.BrandNameFilter != "") {
                    this.FilterBrands(data);
                }
                else {
                    this.setState({ brandsNoFilter: data, brands: data });
                }
            });
    }

    componentDidMount() {
        this.refreshList();
    }

    changeBrandName = (b) => {
        this.setState({ brandName: b.target.value });
    }

    addButton() {
        this.setState({
            brandName: ""
        });
    }

    handleInputChange = (index) => (event) => {
        const newBrands = [...this.state.brands];
        newBrands[index] = { ...newBrands[index], brand_name: event.target.value };
        this.setState({ brands: newBrands, isBrandChanged: true });
    }

    handleInputBlur = (index) => () => {
        if (this.state.isBrandChanged) {
            const brand = this.state.brands[index];
            this.updateClick(brand)
            this.setState({ isBrandChanged: false });
        }

    }

    addClick() {
        const { brandName } = this.state;
        const formData = new FormData();
        formData.append('name', brandName);
        fetch(variables.API_URL + 'CarBrand/AddCarBrand', {
            method: 'POST',
            body: formData,
            headers: {
                'Accept': 'application/json',
            }
        })
            .then(res => res.json())
            .then(result => {
                alert(result);
                this.refreshList();
            })
            .catch(() => {
                alert('Failed to add car brand.');
            });
    }

    updateClick(b) {

        fetch(variables.API_URL + 'CarBrand/EditCarBrand', {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json-patch+json'
            },
            body: JSON.stringify({
                id: b.id,
                name: b.brand_name
            })
        })
            .then(res => {
                if (!res.ok) throw new Error('Network response was not ok');
                return res.json();
            })
            .then(() => {
                alert('Car brand updated successfully');
                this.refreshList();
            })
            .catch(error => {
                console.error('Error during fetch operation:', error);
                alert('Failed to edit car brand.');
            });
    }

    deleteClick(id) {
        if (window.confirm('Are you sure?')) {
            fetch(variables.API_URL + 'CarBrand/DeleteCarBrand/' + id, {
                method: 'DELETE',
                headers: {
                    'Accept': '*/*'
                }
            })
                .then(res => {
                    if (!res.ok) throw new Error('Network response was not ok');
                    alert('Car brand deleted successfully');
                    this.refreshList();
                })
                .catch(error => {
                    console.error('Error during fetch operation:', error);
                    alert('Failed to delete car brand.');
                });
        }
    }
    paginateData = () => {
        const { brands, currentPage, itemsPerPage } = this.state;
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        return brands.slice(startIndex, endIndex);
    }

    // Function to go to previous page
    prevPage = () => {
        this.setState(prevState => ({
            currentPage: prevState.currentPage - 1
        }));
    }

    // Function to go to next page
    nextPage = () => {
        this.setState(prevState => ({
            currentPage: prevState.currentPage + 1
        }));
    }
    render() {
        const {
            brandName
        } = this.state;
        const paginatedBrands = this.paginateData();
        return (
            <div className='content full-height-container'>
                <button type="button"
                    className="btn btn-primary m-2 float-end"
                    data-bs-toggle="modal"
                    data-bs-target="#addModal"
                    onClick={() => this.addButton()}>
                    Add Brand
                </button>
                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>
                                <div className="d-flex flex-row">
                                    <input className="form-control m-2"
                                        onChange={this.changeBrandIdFilter}
                                        placeholder="Filter by ID" />
                                </div>
                                Brand Id
                            </th>
                            <th>
                                <div className="d-flex flex-row">
                                    <input className="form-control m-2"
                                        onChange={this.changeBrandNameFilter}
                                        placeholder="Filter by Name" />
                                </div>
                                Brand Name</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {paginatedBrands.map((b, index) =>
                            <tr key={b.id}>
                                <td>{b.id}</td>
                                <td>
                                    <input type="text" className="form-control"
                                        value={b.brand_name}
                                        onChange={this.handleInputChange(index)}
                                        onBlur={this.handleInputBlur(index)} />
                                </td>
                                <td>
                                    <button type="button"
                                        className="btn btn-light mr-1"
                                        onClick={() => this.deleteClick(b.id)}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                        </svg>
                                    </button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
                <div className="justify-content-center py-1">
                    <button className="btn btn-primary"
                        onClick={this.prevPage}
                        disabled={this.state.currentPage === 1}>
                        Prev
                    </button>
                    <span className="mx-4">{`Page ${this.state.currentPage}`}</span>
                    <button className="btn btn-primary"
                        onClick={this.nextPage}
                        disabled={paginatedBrands.length < this.state.itemsPerPage}>
                        Next
                    </button>
                </div>
                <div className="modal fade" id="addModal" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Add Brand</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"
                                ></button>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">Brand Name</span>
                                    <input type="text" className="form-control"
                                        value={brandName}
                                        onChange={this.changeBrandName} />
                                </div>
                                <button type="button"
                                    className="btn btn-primary float-start"
                                    onClick={() => this.addClick()}>
                                    Add
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}