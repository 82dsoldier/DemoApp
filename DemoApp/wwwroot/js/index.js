(function () {

  var saveTrip = function () {
    if ($("#DeliveryName").val().length === 0) {
      $('#ErrorMessage').html('Please enter a delivery name');
      return;
    }

    if ($('#DepartureTime').val().length === 0) {
      $('#ErrorMessage').html('Please enter a departure time');
      return;
    }

    if ($('#ReturnTime').val().length === 0) {
      $('#ErrorMessage').html('Please ensure you have selected an origin, destination and departure time');
      return;
    }

    $.ajax({
      url: '/deliveries/SaveDelivery',
      method: 'POST',
      dataType: 'JSON',
      contentType: 'application/json',
      headers: {
        'Accept': 'application/json'
      },
      data: JSON.stringify({
        id: 0,
        vehicleId: $('#Vehicles').val(),
        name: $('#DeliveryName').val(),
        departureTime: $('#DepartureTime').val(),
        origin: originId,
        destination: destId
      }),
      complete: function (jqXHR, textStatus, errorThrown) {
        if (jqXHR.status === 201) {
          $('#TrackingNumber').val(jqXHR.responseJSON.trackingNumber);
          $('#ErrorMessage').html('Trip successfully saved');
          $('#SaveButton').prop('disabled', true);
        } else {
          $('#ErrorMessage').html('Data was not successfully saved.  Returned error message was' + jqXHR.textStatus);
        }
      }
    })
  }

  var loadVehicles = function () {
    $.getJSON('/vehicles/getvehicles', function (json) {
      var $select = $('#Vehicles');
      $select.empty();
      $.each(json, function (idx, obj) {
        $select.append($('<option>', { value: obj.id }).text(obj.name));
      });
      $select.trigger('chosen:updated');
    });
  }

  var getPlace = function (placeid) {
    return new Promise(function (resolve, reject) {
      var placeService = new google.maps.places.PlacesService(map);
      placeService.getDetails({ placeId: placeid }, function (place, status) {
        if (status === google.maps.places.PlacesServiceStatus.OK) {
          resolve(place);
        } else {
          reject(status);
        }
      });
    });
  }

  var getRoute = function (origin, destination) {
    return new Promise(function (resolve, reject) {
      var directionsService = new google.maps.DirectionsService(map);
      directionsService.route({
        origin: { 'placeId': origin },
        destination: { 'placeId': destination },
        travelMode: 'DRIVING'
      }, function (response, status) {
        if (status === 'OK') {
          resolve(response);
        } else {
          reject(status);
        }
      });
    });
  }

  var getDeliveries = function (id) {
    $.getJSON('/deliveries/getdeliveriesforvehicle?vehicleId=' + id, function (json) {
      var totalTime = 0;
      $.each(json, function (idx, obj) {
        var origin, dest, rt;
        getPlace(obj.origin)
          .then(function (results) {
            origin = results;
            return getPlace(obj.destination);
          }).then(function (results) {
            dest = results;
            return getRoute(obj.origin, obj.destination);
          }).then(function (results) {
            rt = results;
            var $originCell = $('<td>').html('<a href="#" onclick="centerMap(' + origin.geometry.location.lat() + ', ' + origin.geometry.location.lng() + ')"> ' + origin.name + ' <br /> ' + origin.formatted_address + '</a> ');
            var $destCell = $('<td>').html('<a href="#" onclick="centerMap(' + dest.geometry.location.lat() + ',' + dest.geometry.location.lng() + ')">' + dest.name + '<br />' + dest.formatted_address + '</a>');
            var $departureCell = $('<td>').html(formatTime(new Date(obj.departureTime)));
            var $deliveryCell = $('<td>').html(formatTime(dateAdd(new Date(obj.departureTime), 'second', rt.routes[0].legs[0].duration.value)))
            var $returnCell = $('<td>').html(formatTime(dateAdd(new Date(obj.departureTime), 'second', (rt.routes[0].legs[0].duration.value * 2))));
            totalTime = totalTime + (rt.routes[0].legs[0].duration.value * 2);
            $('#DeliveriesView').find('tbody')
              .append($('<tr>')
                .append($('<td>').html(obj.trackingNumber))
                .append($originCell)
                .append($destCell)
                .append($departureCell)
                .append($deliveryCell)
                .append($returnCell));
          });
      });
    });
  }

  $(document).ready(function () {
    $('#Vehicle').chosen();

    $('#AddVehicle').fancybox({
      type: 'iframe',
      iframe: {
        css: {
          width: '800px',
          height: '600px'
        }
      },
      afterClose: function () {
        loadVehicles();
      }
    });

    $('#SaveButton').click(function () {
      saveTrip();
    });

    $('#ClearButton').click(function () {
      $('#DeliveryName').val('');
      $('#DepartureTime').val('');
      $('#DeliveryTime').val('');
      $('#ReturnTime').val('');
      $('#TrackingNumber').val('');
      $('#origin-input').val('');
      $('#destination-input').val('');
      $('#ErrorMessage').html('');
      $('#SaveButton').prop('disabled', false);
      adh.clearRoute();
      roundTrip = '';
      originId = '';
      destId = '';
    });

    $('#TripsButton').click(function () {
      $('#DeliveriesView > tbody').empty();
      getDeliveries($('#Vehicles').val());
    });

    $('#DepartureTime').datetimepicker();

    $('#DepartureTime').change(function () {
      if (tripTime) {
        var departureDate = new Date($('#DepartureTime').val());
        var returnDate = dateAdd(departureDate, 'second', (tripTime * 2));
        var deliveryTime = formatTime(dateAdd(departureDate, 'second', tripTime));
        var returnTime = formatTime(returnDate);
        $('#ReturnTime').val(returnTime);
        $('#DeliveryTime').val(deliveryTime);
      }
    });

    loadVehicles();

  });
}());

//$(document).ready(function () {
//  $('#Vehicle').chosen();
//  $('#AddVehicle').fancybox({
//    type: 'iframe',
//    iframe: {
//      css: {
//        width: '800px',
//        height: '600px'
//      }
//    },
//    afterClose: function () {
//      loadVehicles();
//    }
//  });

//  $('#SaveButton').click(function () {
//    saveTrip();
//  });

//  $('#ClearButton').click(function () {
//    $('#DeliveryName').val('');
//    $('#RoundTripTime').val('');
//    $('#TrackingNumber').val('');
//    $('#origin-input').val('');
//    $('#destination-input').val('');
//    $('#ErrorMessage').html('');
//    $('#SaveButton').prop('disabled', false);
//    roundTrip = '';
//    originId = '';
//    destId = '';
//  });

//  //$('#TripsButton').click(function () {
    
//  //  $.ajax({
//  //    url: '/deliveries/getdeliveriesforvehicle',
//  //    method: 'GET',
//  //    dataType: 'JSON',
//  //    data: {
//  //      vehicleId: $('#Vehicles').val()
//  //    },
//  //    complete: function (jqXHR, textStatus, errorThrown) {
//  //      if (jqXHR.status === 200) {
//  //        var placeService = new google.maps.places.PlacesService(map);
//  //        var directionsService = new google.maps.DirectionsService(map);

//  //        $.each(jqXHR.responseJSON, function (idx, obj) {
//  //          //var origin, dest, rt;
//  //          var errorMessage = false;

//  //          placeService.getDetails({ placeId: obj.origin }, function (place, status) {
//  //            if (status == google.maps.places.PlacesServiceStatus.OK) {
//  //              var origin = place;
//  //              placeService.getDetails({ placeId: obj.destination }, function (place, status)) {
//  //                if (status == google.maps.places.PlacesServiceStatus.OK) {
//  //                  dest = place;
//  //                  directionsService.route({
//  //                    origin: { 'placeId': obj.origin },
//  //                    destination: { 'placeId': obj.destination },
//  //                    travelMode: 'DRIVING'
//  //                  }, function (response, status) {
//  //                    if (status === 'OK') {
//  //                      rt = formatRoundTrip(response.routes[0].legs[0].duration.value * 2);
//  //                    } else {
//  //                      window.alert('Directions request failed due to ' + status);
//  //                    }
//  //                  });
//  //                } else {
//  //                  $('#ErrorMessage').html('An error occurred retrieving the data');
//  //                  errorMessage = true;
//  //                }
//  //              }
//  //            } else {
//  //              $('#ErrorMessage').html('An error occurred retrieving the data');
//  //              errorMessage = true;
//  //            }
//  //          });



 

            
//  //          if (!origin || origin.length === 0) {
//  //            return;
//  //          }

//  //          $.ajax({
//  //            url: 'https://maps.googleapis.com/maps/api/place/details/json',
//  //            method: 'GET',
//  //            dataType: 'jsonp',
//  //            async: false,
//  //            crossDomain: true,
//  //            data: {
//  //              placeid: obj.destination,
//  //              key: 'AIzaSyBJFvd-3741cFKOCpVXDI2zKIYhRLcmpI8'
//  //            },
//  //            complete: function (jqXHR, textStatus, errorThrown) {
//  //              if (jqXHR.status === 200) {
//  //                dest = jqXHR.responseJSON.response;
//  //              } else {
//  //                $('#ErrorMessage').html('An error occurred fetching the destination address<br />' + response.textStatus);
//  //              }
//  //            }
//  //          });

//  //          if (!dest || dest.length === 0) {
//  //            return;
//  //          }

//  //          $.ajax({
//  //            url: 'https://maps.googleapis.com/maps/api/directions/json',
//  //            method: 'GET',
//  //            async: false,
//  //            dataType: 'jsonp',
//  //            crossDomain: true,
//  //            data: {
//  //              origin: obj.origin,
//  //              destination: obj.destination,
//  //              key: 'AIzaSyBJFvd-3741cFKOCpVXDI2zKIYhRLcmpI8'
//  //            },
//  //            complete: function (jqXHR, textStatus, errorThrown) {
//  //              if (jqXHR.status === 200) {
//  //                rt = jqXHR.responseJSON;
//  //              } else {
//  //                $('#ErrorMessage').html('An error occurred getting the trip data<br />' + response.textStatus);
//  //              }
//  //            }
//  //          });

//  //          if (!rt || rt.length === 0) {
//  //            return;
//  //          }

//  //          var $originCell = $('<td>').html(formatPlace(origin));
//  //          var $destCell = $('<td>').html(formatPlace(dest));
//  //          var $rtCell = $('<td>').html(formatRoundTrip(rt.routes[0].legs[0].duration.value * 2));
//  //          $('#DeliveriesView').find('tbody')
//  //            .append($('<tr>')
//  //              .append($originCell)
//  //              .append($destCell)
//  //              .append($rtCell));
//  //        });
//  //      }
//  //    }
//  //  })
//  //});

//});



//function initMap() {
//  var map = new google.maps.Map(document.getElementById('map'), {
//    mapTypeControl: false,
//    center: { lat: 39.580745, lng: -119.723402 },
//    zoom: 13
//  });

//  new AutocompleteDirectionsHandler(map);
//}


