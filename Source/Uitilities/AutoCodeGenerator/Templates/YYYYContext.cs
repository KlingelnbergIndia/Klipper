        public IMongoCollection<XXXX> XXXXs => _database.GetCollection<XXXX>("XXXXs");
		
            services.AddTransient<IXXXXRepository, XXXXRepository>();
