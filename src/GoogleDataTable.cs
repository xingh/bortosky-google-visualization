///////////////////////////////////////////////////////////
//  GoogleDataTable.cs
//  Implementation of the Class GoogleDataTable
//  Generated by Enterprise Architect
//  Created on:      12-Feb-2009 5:20:03 PM
//  Original author: Gary
// 
//  Copyright (c) 2009 Gary Bortosky. All rights reserved. 
// 
//  This library is free software; you can redistribute it and/or modify it
//  under the terms of the New BSD License, a copy of which should have
//  been delivered along with this distribution.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
//  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
//  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
//  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
///////////////////////////////////////////////////////////




using System.Data;
using Bortosky.Google.Visualization;
using System.IO;
using Bortosky.Google.Visualization.Columns;
using System.Collections.Generic;
namespace Bortosky.Google.Visualization {
	public class GoogleDataTable {

		private DataTable subjectTable;
		private StreamWriter writer;
		private List<GoogleDataColumn> columns;

		/// 
		/// <param name="table">The table to serialize</param>
		public GoogleDataTable(DataTable table){
            this.subjectTable = table;
		}

		/// <summary>
		/// Columns of DateTime Type can be specified as one of the various Google date
		/// types for specialized serialization. The implementation makes use of the
		/// ExtendedProperties of the column, namely the "GoogleDateType" key.
		/// </summary>
		/// <param name="column">The DataColumn of DateTime Type whose Google Data Type is
		/// to be specified.</param>
		/// <param name="dateType">The Google Date Type of the passed column</param>
		public static void SetGoogleDateType(DataColumn column, GoogleDateType dateType){
            column.ExtendedProperties.Add("GoogleDateType", dateType);
		}

		/// 
		/// <param name="columnName">The DataColumn of DateTime Type whose Google Data Type
		/// is to be specified.</param>
		/// <param name="dateType">The Google Date Type of the passed column</param>
		public void SetGoogleDateType(string columnName, GoogleDateType dateType){
            SetGoogleDateType(subjectTable.Columns[columnName], dateType);
		}

		/// 
		/// <param name="stream">The stream to which to serialize the subject
		/// DataTable</param>
		public void WriteJson(Stream stream){
            int colCount = 0, rowCount = 0;
            this.writer = new StreamWriter(stream);
            this.columns = new List<GoogleDataColumn>();
            foreach (DataColumn c in this.subjectTable.Columns)
                this.columns.Add(GoogleDataColumn.CreateGoogleDataColumn(c));
            this.writer.Write("{cols: [");
            foreach (GoogleDataColumn gc in this.columns)
                writer.Write("{0}{1}", colCount++ == 0 ? "" : ", ", gc.SerializedColumnIdentifier);
            this.writer.Write("], rows: [");
            foreach (DataRow r in this.subjectTable.Rows)
            {
                writer.Write("{0}{{c: [", rowCount++ == 0 ? "" : ", ");
                colCount = 0;
                foreach (GoogleDataColumn gc in this.columns)
                    writer.Write("{0}{{v: {1}}}", colCount++ == 0 ? "" : ", ", gc.SerializedValue(r));
                writer.Write("]}");
            }
            writer.Write("]}");
            writer.Flush();
		}

	}//end GoogleDataTable

}//end namespace Visualization