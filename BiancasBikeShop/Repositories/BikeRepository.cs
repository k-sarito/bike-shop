using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            var bikes = new List<Bike>();
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT b.Id, b.Brand, b.Color, b.OwnerId, b.BikeTypeId, o.Name AS OwnerName, o.Address, o.Email, o.Telephone, bt.Name AS TypeName
                        FROM Bike as b
                        LEFT JOIN Owner as o ON o.Id = b.OwnerId
                        LEFT JOIN BikeType as bt ON bt.Id = b.BikeTypeId";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Bike bike = new Bike
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                                },
                                BikeType = new BikeType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("TypeName"))
                                }

                            };
                            bikes.Add(bike);
                        }
                    }
                }
            }
            return bikes;
        }

        public Bike GetBikeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT b.Id, b.Brand, Color, b.OwnerId, b.BikeTypeId, o.Name AS OwnerName, o.Address, o.Email, o.Telephone, bt.Name AS TypeName, w.Id AS WorkId, w.Description, w.DateInitiated, w.DateCompleted
                        FROM Bike as b
                        LEFT JOIN Owner as o ON o.Id = b.OwnerId
                        LEFT JOIN BikeType as bt ON bt.Id = b.BikeTypeId
                        LEFT JOIN WorkOrder as w ON w.BikeId = b.Id
                        WHERE b.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Bike bike = null;

                    while (reader.Read())
                    {
                        if (bike == null)
                        {
                            bike = new Bike()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                                },
                                BikeType = new BikeType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("TypeName"))
                                },
                                WorkOrders = new List<WorkOrder>()
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("WorkId")))
                        {
                            WorkOrder workOrder = new WorkOrder()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("WorkId")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                DateCompleted = reader.IsDBNull(reader.GetOrdinal("DateCompleted")) ? null : reader.GetDateTime(reader.GetOrdinal("DateCompleted"))
                            };
                            bike.WorkOrders.Add(workOrder);
                        }
                    }

                    reader.Close();

                    return bike;
                }
            }            
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;
        }
    }
}
